using Octokit;
using System;

namespace Estranged.Lfs.Authenticator.GitHub
{
    internal interface IGitHubClientFactory
    {
        IRepositoriesClient CreateClient(Uri baseAddress, string username, string password);
    }
}