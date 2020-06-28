using Microsoft.AspNetCore.Builder;
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

            string lfsBucket = config["LFS_BUCKET"] ?? throw new InvalidOperationException("Missing environment variable LFS_BUCKET");
            string lfsUsername = config["LFS_USERNAME"] ?? throw new InvalidOperationException("Missing environment variable LFS_USERNAME");
            string lfsPassword = config["LFS_PASSWORD"] ?? throw new InvalidOperationException("Missing environment variable LFS_PASSWORD");
            bool.TryParse(config["S3_ACCELERATION"] ?? "false", out bool s3Acceleration);

            services.AddSingleton<IS3BlobAdapterConfig>(x => new S3BlobAdapterConfig { Bucket = lfsBucket });
            services.AddSingleton<IAuthenticator>(x => new DictionaryAuthenticator(new Dictionary<string, string> { { lfsUsername, lfsPassword } }));

            services.AddSingleton<IAmazonS3>(x => new AmazonS3Client(new AmazonS3Config { UseAccelerateEndpoint = s3Acceleration }));
            services.AddSingleton<IBlobAdapter, S3BlobAdapter>();
            services.AddLfs();

            services.AddLogging(x => x.AddLambdaLogger());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
