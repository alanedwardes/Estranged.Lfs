using Estranged.Lfs.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Estranged.Lfs.Api.Filters
{
    public class BasicAuthFilter : IAsyncActionFilter
    {
        private readonly IAuthenticator authenticator;

        public BasicAuthFilter(IAuthenticator authenticator)
        {
            this.authenticator = authenticator;
        }

        public string AuthorizationHeader => "Authorization";
        public string BasicPrefix => "Basic";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IHeaderDictionary headers = context.HttpContext.Request.Headers;
            if (!headers.ContainsKey(AuthorizationHeader))
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            string[] authValues = headers[AuthorizationHeader].ToArray();
            if (authValues.Length != 1)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            string auth = authValues.Single();
            if (!auth.StartsWith(BasicPrefix))
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            auth = auth.Substring(BasicPrefix.Length).Trim();

            byte[] decoded = Convert.FromBase64String(auth);
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");

            string[] authPair = iso.GetString(decoded).Split(':');
            if (!authenticator.Authenticate(authPair[0], authPair[1]))
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            await next();
        }
    }
}
