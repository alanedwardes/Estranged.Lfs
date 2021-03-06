using Estranged.Lfs.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Estranged.Lfs.Authenticator.BitBucket
{
    internal sealed class BitBucketAuthenticator : IAuthenticator
    {
        private readonly IBitBucketAuthenticatorConfig config;
        private readonly IBitBucketClientFactory clientFactory;

        public BitBucketAuthenticator(IBitBucketAuthenticatorConfig config, IBitBucketClientFactory clientFactory)
        {
            this.config = config;
            this.clientFactory = clientFactory;
        }

        public async Task Authenticate(string username, string password, LfsPermission requiredPermission, CancellationToken token)
        {
            var client = clientFactory.CreateClient(config.BaseAddress, username, password);

            // Check that the user can access the repository
            await client.GetRepository(config.Workspace, config.Repository, token);
        }
    }
}
