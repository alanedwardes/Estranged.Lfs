using Amazon.S3;
using Estranged.Lfs.Adapter.S3;
using Estranged.Lfs.Api;
using Estranged.Lfs.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Estranged.Lfs.Hosting.AspNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLogging(x =>
            {
                x.AddConsole();
                x.AddDebug();
            });

            builder.Services.AddSingleton<IAmazonS3, AmazonS3Client>();
            builder.Services.AddLfsS3Adapter(new S3BlobAdapterConfig { Bucket = "estranged-lfs-test" }, new AmazonS3Client());
            builder.Services.AddSingleton<IAuthenticator>(x => new DictionaryAuthenticator(new Dictionary<string, string> { { "usernametest", "passwordtest" } }));
            builder.Services.AddLfsApi();

            var app = builder.Build();

            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}