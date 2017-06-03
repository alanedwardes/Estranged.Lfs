using Estranged.Lfs.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Estranged.Lfs.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLfs(this IServiceCollection services)
        {
            services.AddMvcCore()
                    .AddGitLfs();

            services.AddLfsData();
            return services;
        }
    }
}
