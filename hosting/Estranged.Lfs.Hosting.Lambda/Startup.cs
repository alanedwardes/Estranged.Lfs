using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Estranged.Lfs.Api;
using Estranged.Lfs.Adapter.S3;
using Amazon.S3;
using Estranged.Lfs.Data;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;

namespace Estranged.Lfs.Hosting.Lambda
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            string LfsBucket = config["LFS_BUCKET"] ?? throw new InvalidOperationException("Missing environment variable LFS_BUCKET");
            string LfsUsername = config["LFS_USERNAME"] ?? throw new InvalidOperationException("Missing environment variable LFS_USERNAME");
            string LfsPassword = config["LFS_PASSWORD"] ?? throw new InvalidOperationException("Missing environment variable LFS_PASSWORD");

            services.AddSingleton<IS3BlobAdapterConfig>(x => new S3BlobAdapterConfig { Bucket = LfsBucket });
            services.AddSingleton<IAuthenticator>(x => new DictionaryAuthenticator(new Dictionary<string, string> { { LfsUsername, LfsPassword } }));

            services.AddSingleton<IAmazonS3>(x => new AmazonS3Client());
            services.AddSingleton<IBlobAdapter, S3BlobAdapter>();
            services.AddLfs();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLambdaLogger();
            app.UseMvc();
        }
    }
}
