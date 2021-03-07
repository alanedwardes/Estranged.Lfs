using Estranged.Lfs.Authenticator.BitBucket;
using Estranged.Lfs.Authenticator.BitBucket.Entities;
using Estranged.Lfs.Data;
using Moq;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Estranged.Lfs.Tests.Authenticator.BitBucket
{
    public class BitBucketAuthenticatorTests : IDisposable
    {
        private readonly MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

        public void Dispose() => mockRepository.VerifyAll();

        [Fact]
        public async Task TestAuthenticatePrivateRepositorySuccessful()
        {
            var factory = mockRepository.Create<IBitBucketClientFactory>();
            var client = mockRepository.Create<IBitBucketClient>();

            factory.Setup(x => x.CreateClient(new Uri("https://www.example.com/"), "username", "password"))
                   .Returns(client.Object);

            client.Setup(x => x.GetRepository("workspace", "repository", CancellationToken.None))
                  .ReturnsAsync(new Repository());

            var authenticator = new BitBucketAuthenticator(new BitBucketAuthenticatorConfig
            {
                Workspace = "workspace",
                Repository = "repository",
                BaseAddress = new Uri("https://www.example.com/")
            }, factory.Object);

            await authenticator.Authenticate("username", "password", LfsPermission.Read, CancellationToken.None);
            await authenticator.Authenticate("username", "password", LfsPermission.Write, CancellationToken.None);
        }

        [Fact]
        public async Task TestAuthenticateInvalidRepository()
        {
            var factory = mockRepository.Create<IBitBucketClientFactory>();
            var client = mockRepository.Create<IBitBucketClient>();

            factory.Setup(x => x.CreateClient(new Uri("https://www.example.com/"), "username", "password"))
                   .Returns(client.Object);

            client.Setup(x => x.GetRepository("workspace", "repository", CancellationToken.None))
                  .ThrowsAsync(new HttpRequestException());

            var authenticator = new BitBucketAuthenticator(new BitBucketAuthenticatorConfig
            {
                Workspace = "workspace",
                Repository = "repository",
                BaseAddress = new Uri("https://www.example.com/")
            }, factory.Object);

            await Assert.ThrowsAsync<HttpRequestException>(() => authenticator.Authenticate("username", "password", LfsPermission.Read, CancellationToken.None));
        }
    }
}
