using Amazon.S3;
using Estranged.Lfs.Adapter.S3;
using Estranged.Lfs.Api;
using Estranged.Lfs.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            const string LfsBucket = "estranged-lfs-test";
            const string S3AccessKeyId = "";
            const string S3AccessKeySecret = "";
            const string S3Region = "";
            const string S3ServiceURL = "";
            if (!string.IsNullOrWhiteSpace(S3ServiceURL) && !string.IsNullOrWhiteSpace(S3Region) && !string.IsNullOrWhiteSpace(S3AccessKeyId) && !string.IsNullOrWhiteSpace(S3AccessKeySecret))
            {
                services.AddLfsS3Adapter(new S3BlobAdapterConfig { Bucket = LfsBucket }, new AmazonS3Client(S3AccessKeyId, S3AccessKeySecret, new AmazonS3Config { ServiceURL = S3ServiceURL, AuthenticationRegion = S3Region, SignatureVersion = "V4" }));
            }
            else
            {
                services.AddLfsS3Adapter(new S3BlobAdapterConfig { Bucket = LfsBucket }, new AmazonS3Client());
            }
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