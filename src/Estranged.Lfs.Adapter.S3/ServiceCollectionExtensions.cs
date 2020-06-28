using Estranged.Lfs.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Estranged.Lfs.Adapter.S3
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLfsS3Adapter(this IServiceCollection services, IS3BlobAdapterConfig config)
        {
            return services.AddSingleton<IBlobAdapter, S3BlobAdapter>().AddSingleton(config);
        }
    }
}
