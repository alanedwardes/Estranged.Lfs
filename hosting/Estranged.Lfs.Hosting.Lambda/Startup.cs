using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Estranged.Lfs.Api;
using Estranged.Lfs.Adapter.S3;
using Amazon.S3;
using Estranged.Lfs.Data;
using System.Collections.Generic;

namespace Estranged.Lfs.Hosting.Lambda
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IS3BlobAdapterConfig>(x => new S3BlobAdapterConfig
            {
                Bucket = "estranged-lfs-test"
            });

            services.AddSingleton<IAmazonS3>(x => new AmazonS3Client());
            services.AddSingleton<IBlobAdapter, S3BlobAdapter>();
            services.AddSingleton<IAuthenticator>(x => new DictionaryAuthenticator(new Dictionary<string, string> { { "usernametest", "passwordtest" } }));
            services.AddLfs();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLambdaLogger();
            app.UseMvc();
        }
    }
}
