using Estranged.Lfs.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Estranged.Lfs.Authenticator.GitHub
{
    internal sealed class GitHubAuthenticator : IAuthenticator
    {
        private readonly IGitHubAuthenticatorConfig config;
        private readonly IGitHubClientFactory clientFactory;

        public GitHubAuthenticator(IGitHubAuthenticatorConfig config, IGitHubClientFactory clientFactory)
        {
            this.config = config;
            this.clientFactory = clientFactory;
        }

        public async Task Authenticate(string username, string password, LfsPermission requiredPermission, CancellationToken token)
        {
            var client = clientFactory.CreateClient(config.BaseAddress, username, password);

            var repository = await client.Get(config.Organisation, config.Repository);

            LfsPermission actualPermission = LfsPermission.None;

            if (repository.Permissions.Pull)
            {
                actualPermission |= LfsPermission.Read;
            }

            if (repository.Permissions.Push)
            {
                actualPermission |= LfsPermission.Write;
            }

            if (!actualPermission.HasFlag(requiredPermission))
            {
                throw new InvalidOperationException($"User {username} doesn't have permission {requiredPermission} for repository {config.Organisation}/{config.Repository} (actual: {actualPermission})");
            }
        }
    }
}
