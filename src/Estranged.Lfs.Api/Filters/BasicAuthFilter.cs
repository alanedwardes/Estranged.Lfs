using Estranged.Lfs.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;

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

        private void Unauthorised(ActionExecutingContext context)
        {
            context.Result = new StatusCodeResult(401);
        }

        private (string Username, string Password) GetCredentials(IHeaderDictionary headers)
        {
            if (!headers.ContainsKey(AuthorizationHeader))
            {
                throw new InvalidOperationException("No Authorization header found.");
            }

            string[] authValues = headers[AuthorizationHeader].ToArray();
            if (authValues.Length != 1)
            {
                throw new InvalidOperationException("More than one Authorization header found.");
            }

            string auth = authValues.Single();
            if (!auth.StartsWith(BasicPrefix))
            {
                throw new InvalidOperationException("Authorization header is not Basic.");
            }

            auth = auth.Substring(BasicPrefix.Length).Trim();

            byte[] decoded = Convert.FromBase64String(auth);
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");

            string[] authPair = iso.GetString(decoded).Split(':');

            return (authPair[0], authPair[1]);
        }

        private LfsPermission GetRequiredPermission(HttpRequest request) => request.Method.ToUpper() == "GET" ? LfsPermission.Read : LfsPermission.Write;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string username;
            string password;
            try
            {
                (username, password) = GetCredentials(context.HttpContext.Request.Headers);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error getting Basic credentials");
                Unauthorised(context);
                return;
            }

            try
            {
                await authenticator.Authenticate(username, password, GetRequiredPermission(context.HttpContext.Request), CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error from {authenticator.GetType().Name}");
                Unauthorised(context);
                return;
            }

            await next();
        }
    }
}
