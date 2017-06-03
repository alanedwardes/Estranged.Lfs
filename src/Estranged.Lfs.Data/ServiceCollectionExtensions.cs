using Microsoft.Extensions.DependencyInjection;

namespace Estranged.Lfs.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLfsData(this IServiceCollection services)
        {
            services.AddTransient<IObjectManager, ObjectManager>();
            return services;
        }
    }
}
