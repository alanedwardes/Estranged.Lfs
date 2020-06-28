using Estranged.Lfs.Data;
using Octokit;
using System;
using System.Threading.Tasks;

namespace Estranged.Lfs.Authenticator.GitHub
{
    public class GitHubAuthenticator : IAuthenticator
    {
        private readonly IGitHubAuthenticatorConfig config;

        public GitHubAuthenticator(IGitHubAuthenticatorConfig config)
        {
            this.config = config;
        }

        private IRepositoriesClient CreateClient(string username, string password)
        {
            var assemblyName = GetType().Assembly.GetName();

            var client = new GitHubClient(new Connection(new ProductHeaderValue(assemblyName.Name, assemblyName.Version.ToString(3))))
            {
                Credentials = new Credentials(username, password)
            };

            return client.Repository;
        }

        public async Task Authenticate(string username, string password, LfsPermission requiredPermission)
        {
            var client = CreateClient(username, password);

            var repository = await client.Get(config.Organisation, config.Repository);

            LfsPermission actualPermission = LfsPermission.None;

            if (repository.Permissions.Pull)
            {
                actualPermission &= LfsPermission.Read;
            }

            if (repository.Permissions.Push)
            {
                actualPermission &= LfsPermission.Write;
            }

            if (!actualPermission.HasFlag(requiredPermission))
            {
                throw new InvalidOperationException($"User {username} doesn't have permission {requiredPermission} for repository {config.Organisation}/{config.Repository} (actual: {actualPermission})");
            }
        }
    }
}
