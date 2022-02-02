using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Estranged.Lfs.Api;
using Estranged.Lfs.Adapter.Azure.Blob;
using Microsoft.Extensions.Configuration;
using Estranged.Lfs.Data;
using Estranged.Lfs.Authenticator.GitHub;
using System.Collections.Generic;

namespace Estranged.Lfs.Hosting.AspNet
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            IConfiguration credentials = new ConfigurationBuilder()
                .AddJsonFile("credentials.json")
                .Build();

            services.AddLogging(x =>
            {
                x.AddConsole();
                x.AddDebug();
            });

            services.AddLfsGitHubAuthenticator(new GitHubAuthenticatorConfig 
            { 
                Organisation = credentials["GitHubOrganisation"],
                Repository = credentials["GitHubRepository"] 
            });

            services.AddLfsAzureBlobAdapter(new AzureBlobAdapterConfig
            {
                ConnectionString = credentials["LfsAzureStorageConnectionString"],
                ContainerName = credentials["LfsAzureStorageContainerName"]
            });

            services.AddSingleton<IAuthenticator>(
                x => new DictionaryAuthenticator(
                    new Dictionary<string, string> {
                        { credentials["AuthUserName"], credentials["AuthPassword"] }
                    }));

            services.AddLfsApi();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
