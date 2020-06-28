using Estranged.Lfs.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Estranged.Lfs.Api.Filters
{
    public class BasicAuthFilter : IAsyncActionFilter
    {
        private readonly ILogger<BasicAuthFilter> logger;
        private readonly IAuthenticator authenticator;

        public BasicAuthFilter(ILogger<BasicAuthFilter> logger, IAuthenticator authenticator)
        {
            this.logger = logger;
            this.authenticator = authenticator;
        }

        public string AuthorizationHeader => "Authorization";
        public string BasicPrefix => "Basic";

        private void Deny(ActionExecutingContext context)
        {
            context.Result = new StatusCodeResult(401);
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IHeaderDictionary headers = context.HttpContext.Request.Headers;
            if (!headers.ContainsKey(AuthorizationHeader))
            {
                Deny(context);
                return;
            }

            string[] authValues = headers[AuthorizationHeader].ToArray();
            if (authValues.Length != 1)
            {
                Deny(context);
                return;
            }

            string auth = authValues.Single();
            if (!auth.StartsWith(BasicPrefix))
            {
                Deny(context);
                return;
            }

            auth = auth.Substring(BasicPrefix.Length).Trim();

            byte[] decoded = Convert.FromBase64String(auth);
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");

            string[] authPair = iso.GetString(decoded).Split(':');

            LfsPermission requiredPermission = context.HttpContext.Request.Method.ToUpper() == "GET" ? LfsPermission.Read : LfsPermission.Write;
            try
            {
                await authenticator.Authenticate(authPair[0], authPair[1], requiredPermission).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                logger.LogError(e, "ERror when authenticating");
                Deny(context);
            }

            await next();
        }
    }
}
