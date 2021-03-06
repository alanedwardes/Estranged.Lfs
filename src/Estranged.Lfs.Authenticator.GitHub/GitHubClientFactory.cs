using Octokit;
using System;

namespace Estranged.Lfs.Authenticator.GitHub
{
    internal sealed class GitHubClientFactory : IGitHubClientFactory
    {
        public IRepositoriesClient CreateClient(Uri baseAddress, string username, string password)
        {
            var assemblyName = GetType().Assembly.GetName();

            var productHeaderValue = new ProductHeaderValue(assemblyName.Name, assemblyName.Version.ToString(3));

            var client = new GitHubClient(new Connection(productHeaderValue, baseAddress))
            {
                Credentials = new Credentials(username, password)
            };

            return client.Repository;
        }
    }
}
