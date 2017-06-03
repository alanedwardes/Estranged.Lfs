using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Estranged.Lfs.Api;

namespace Estranged.Lfs.Hosting.Lambda
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLfs();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLambdaLogger();
            app.UseMvc();
        }
    }
}
