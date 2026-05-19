using Estranged.Lfs.Authenticator.GitHub;
using Estranged.Lfs.Data;
using Moq;
using Octokit;
using System;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Estranged.Lfs.Tests.Authenticator.GitHub
{
    public class GitHubAuthenticatorTests : IDisposable
    {
        private readonly MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

        public void Dispose() => mockRepository.VerifyAll();

        private Repository CreateMockRepository(bool admin, bool canPush, bool canPull)
        {
            // Create an "empty" Repository
            var repo = (Repository)Activator.CreateInstance(typeof(Repository), true);

            // Access the "Permissions" private property via reflection
            var property = typeof(Repository).GetProperty("Permissions", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (property != null && property.CanWrite)
            {
                property.SetValue(repo, new RepositoryPermissions(admin, admin, canPush, false, canPull));
            }

            return repo;
        }

        [Fact]
        public async Task TestAuthenticatePrivateRepositoryReadWriteSuccessful()
        {
            var factory = mockRepository.Create<IGitHubClientFactory>();
            var client = mockRepository.Create<IRepositoriesClient>();

            factory.Setup(x => x.CreateClient(new Uri("https://www.example.com/"), "username", "password"))
                   .Returns(client.Object);

            client.Setup(x => x.Get("organisation", "repository"))
                  .ReturnsAsync(CreateMockRepository(false, true, true));

            var authenticator = new GitHubAuthenticator(new GitHubAuthenticatorConfig
            {
                Organisation = "organisation",
                Repository = "repository",
                BaseAddress = new Uri("https://www.example.com/")
            }, factory.Object);

            await authenticator.Authenticate("username", "password", LfsPermission.Read, CancellationToken.None);
            await authenticator.Authenticate("username", "password", LfsPermission.Write, CancellationToken.None);
        }

        [Fact]
        public async Task TestAuthenticatePublicRepositoryWriteUnsuccessful()
        {
            var factory = mockRepository.Create<IGitHubClientFactory>();
            var client = mockRepository.Create<IRepositoriesClient>();

            factory.Setup(x => x.CreateClient(new Uri("https://www.example.com/"), "username", "password"))
                   .Returns(client.Object);

            client.Setup(x => x.Get("organisation", "repository"))
                  .ReturnsAsync(CreateMockRepository(false, false, true));

            var authenticator = new GitHubAuthenticator(new GitHubAuthenticatorConfig
            {
                Organisation = "organisation",
                Repository = "repository",
                BaseAddress = new Uri("https://www.example.com/")
            }, factory.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                authenticator.Authenticate("username", "password", LfsPermission.Write, CancellationToken.None));
        }

        [Fact]
        public async Task TestAuthenticateInvalidRepository()
        {
            var factory = mockRepository.Create<IGitHubClientFactory>();
            var client = mockRepository.Create<IRepositoriesClient>();

            factory.Setup(x => x.CreateClient(new Uri("https://www.example.com/"), "username", "password"))
                   .Returns(client.Object);

            client.Setup(x => x.Get("organisation", "repository"))
                  .ThrowsAsync(new NotFoundException("message", HttpStatusCode.NotFound));

            var authenticator = new GitHubAuthenticator(new GitHubAuthenticatorConfig
            {
                Organisation = "organisation",
                Repository = "repository",
                BaseAddress = new Uri("https://www.example.com/")
            }, factory.Object);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                authenticator.Authenticate("username", "password", LfsPermission.Write, CancellationToken.None));
        }
    }
}