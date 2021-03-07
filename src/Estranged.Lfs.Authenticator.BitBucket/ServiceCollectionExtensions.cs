using Estranged.Lfs.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Estranged.Lfs.Authenticator.BitBucket
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLfsBitBucketAuthenticator(this IServiceCollection services, IBitBucketAuthenticatorConfig config)
        {
            return services.AddSingleton<IAuthenticator, BitBucketAuthenticator>()
                           .AddSingleton<IBitBucketClientFactory, BitBucketClientFactory>()
                           .AddSingleton(config);
        }
    }
}
