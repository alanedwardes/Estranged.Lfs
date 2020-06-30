using Estranged.Lfs.Data;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Estranged.Lfs.Authenticator.BitBucket
{
    internal sealed class BitBucketAuthenticator : IAuthenticator
    {
        private readonly IBitBucketAuthenticatorConfig config;

        public BitBucketAuthenticator(IBitBucketAuthenticatorConfig config)
        {
            this.config = config;
        }

        private IBitBucketClient CreateClient(string username, string password)
        {
            var client = new HttpClient { BaseAddress = config.BaseAddress };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));
            return new BitBucketClient(client);
        }

        public async Task Authenticate(string username, string password, LfsPermission requiredPermission, CancellationToken token)
        {
            var client = CreateClient(username, password);

            // Check that the user can access the repository
            await client.GetRepository(config.Workspace, config.Repository, token);
        }
    }
}
