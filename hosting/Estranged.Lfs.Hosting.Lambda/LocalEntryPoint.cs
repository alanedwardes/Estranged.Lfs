using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Estranged.Lfs.Hosting.Lambda
{
    public class LocalEntryPoint
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var startup = new Startup();
            startup.ConfigureServices(builder.Services);

            var app = builder.Build();
            startup.Configure(app);

            app.Run();
        }
    }
}