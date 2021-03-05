using Estranged.Lfs.Authenticator.GitHub;
using Estranged.Lfs.Data;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Estranged.Lfs.Tests.Authenticator.GitHub
{
    public class GitHubAuthenticatorTests
    {
        private IAuthenticator CreateAuthenticator(IGitHubAuthenticatorConfig config)
        {
            return new ServiceCollection().AddLfsGitHubAuthenticator(config).BuildServiceProvider().GetRequiredService<IAuthenticator>();
        }

        [Fact(Skip = "Should use mock data")]
        public async Task TestAuthenticatePrivateRepositoryReadWriteSuccessful()
        {
            var authenticator = CreateAuthenticator(new GitHubAuthenticatorConfig
            {
                Organisation = "alanedwardes",
                Repository = "test"
            });

            var username = "mock";
            var password = "mock";
            await authenticator.Authenticate(username, password, LfsPermission.Read, CancellationToken.None);
            await authenticator.Authenticate(username, password, LfsPermission.Write, CancellationToken.None);
        }

        [Fact(Skip = "Should use mock data")]
        public async Task TestAuthenticatePublicRepositoryReadSuccessful()
        {
            var authenticator = CreateAuthenticator(new GitHubAuthenticatorConfig
            {
                Organisation = "dotnet",
                Repository = "runtime"
            });

            var username = "mock";
            var password = "mock";
            await authenticator.Authenticate(username, password, LfsPermission.Read, CancellationToken.None);
        }

        [Fact(Skip = "Should use mock data")]
        public async Task TestAuthenticatePublicRepositoryWriteUnsuccessful()
        {
            var authenticator = CreateAuthenticator(new GitHubAuthenticatorConfig
            {
                Organisation = "dotnet",
                Repository = "runtime"
            });

            var username = "mock";
            var password = "mock";
            await Assert.ThrowsAsync<InvalidOperationException>(() => authenticator.Authenticate(username, password, LfsPermission.Write, CancellationToken.None));
        }

        [Fact(Skip = "Should use mock data")]
        public async Task TestAuthenticateInvalidRepository()
        {
            var authenticator = CreateAuthenticator(new GitHubAuthenticatorConfig
            {
                Organisation = "f5155156-f2e5-49da-b93f-c9a0f409cf4c",
                Repository = "ff5deca4-bcc6-4857-800a-90a79c086e0b"
            });

            var username = "mock";
            var password = "mock";
            await Assert.ThrowsAsync<NotFoundException>(() => authenticator.Authenticate(username, password, LfsPermission.Write, CancellationToken.None));
        }
    }
}
