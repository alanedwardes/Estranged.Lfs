using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Estranged.Lfs.Api;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Estranged.Lfs.Adapter.S3;
using Estranged.Lfs.Data;
using System.Collections.Generic;

namespace Estranged.Lfs.Hosting.AspNet
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            IConfiguration credentials = new ConfigurationBuilder().AddJsonFile("credentials.json").Build();

            services.AddLogging(x =>
            {
                x.AddConsole();
                x.AddDebug();
            });

            services.AddSingleton<IAmazonS3, AmazonS3Client>();
            services.AddLfsS3Adapter(new S3BlobAdapterConfig
            {
                Bucket = "estranged-lfs-test"
            });
            services.AddSingleton<IAuthenticator>(x => new DictionaryAuthenticator(new Dictionary<string, string> { { "usernametest", "passwordtest" } }));
            services.AddLfsApi();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
