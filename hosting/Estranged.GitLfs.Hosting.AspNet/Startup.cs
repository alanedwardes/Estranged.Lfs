using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Estranged.GitLfs.Api;
using Estranged.GitLfs.Storage;
using Amazon.S3;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;

namespace Estranged.GitLfs.Hosting.AspNet
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IS3BlobStoreConfig>(x => new S3BlobStoreConfig
            {
                Bucket = "estranged-lfs-test"
            });

            IConfiguration credentials = new ConfigurationBuilder().AddJsonFile("credentials.json").Build();

            services.AddSingleton<IAmazonS3>(x => new AmazonS3Client(new BasicAWSCredentials(credentials["s3:key"], credentials["s3:secret"]), Amazon.RegionEndpoint.EUWest2));
            services.AddSingleton<IBlobStore, S3BlobStore>();
            services.AddMvc().AddGitLfs();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Trace);
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
