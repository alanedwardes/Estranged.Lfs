using Estranged.Lfs.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Estranged.Lfs.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLfsApi(this IServiceCollection services)
        {
            services.AddMvcCore()
                    .AddGitLfs();

            services.AddLfsData();
            return services;
        }
    }
}
