using Estranged.Lfs.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Estranged.Lfs.Authenticator.GitHub
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLfsGitHubAuthenticator(this IServiceCollection services, IGitHubAuthenticatorConfig config)
        {
            return services.AddSingleton<IAuthenticator, GitHubAuthenticator>()
                           .AddSingleton<IGitHubClientFactory, GitHubClientFactory>()
                           .AddSingleton(config);
        }
    }
}
