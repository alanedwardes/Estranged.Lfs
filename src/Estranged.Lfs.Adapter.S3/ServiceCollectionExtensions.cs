using Amazon.S3;
using Estranged.Lfs.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Estranged.Lfs.Adapter.S3
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLfsS3Adapter(this IServiceCollection services, IS3BlobAdapterConfig config, IAmazonS3 amazonS3)
        {
            return services.AddSingleton<IBlobAdapter, S3BlobAdapter>().AddSingleton(amazonS3).AddSingleton(config);
        }
    }
}
