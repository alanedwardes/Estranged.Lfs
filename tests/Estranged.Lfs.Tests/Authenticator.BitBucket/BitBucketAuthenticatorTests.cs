using Estranged.Lfs.Authenticator.BitBucket;
using Estranged.Lfs.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Estranged.Lfs.Tests.Authenticator.BitBucket
{
    public class BitBucketAuthenticatorTests
    {
        private IAuthenticator CreateAuthenticator(IBitBucketAuthenticatorConfig config)
        {
            return new ServiceCollection().AddLfsBitBucketAuthenticator(config).BuildServiceProvider().GetRequiredService<IAuthenticator>();
        }

        [Fact(Skip = "Should use mock data")]
        public async Task TestAuthenticatePrivateRepositorySuccessful()
        {
            var authenticator = CreateAuthenticator(new BitBucketAuthenticatorConfig
            {
                Workspace = "alanedwardes",
                Repository = "test-private"
            });

            var username = "mock";
            var password = "mock";
            await authenticator.Authenticate(username, password, LfsPermission.Read, CancellationToken.None);
            await authenticator.Authenticate(username, password, LfsPermission.Write, CancellationToken.None);
        }

        [Fact(Skip = "Should use mock data")]
        public async Task TestAuthenticatePublicRepositorySuccessful()
        {
            var authenticator = CreateAuthenticator(new BitBucketAuthenticatorConfig
            {
                Workspace = "alanedwardes",
                Repository = "test-public"
            });

            var username = "mock";
            var password = "mock";
            await authenticator.Authenticate(username, password, LfsPermission.Read, CancellationToken.None);
        }

        [Fact(Skip = "Should use mock data")]
        public async Task TestAuthenticateInvalidRepository()
        {
            var authenticator = CreateAuthenticator(new BitBucketAuthenticatorConfig
            {
                Workspace = "f5155156-f2e5-49da-b93f-c9a0f409cf4c",
                Repository = "ff5deca4-bcc6-4857-800a-90a79c086e0b"
            });

            var username = "mock";
            var password = "mock";
            await Assert.ThrowsAsync<HttpRequestException>(() => authenticator.Authenticate(username, password, LfsPermission.Read, CancellationToken.None));
        }
    }
}
