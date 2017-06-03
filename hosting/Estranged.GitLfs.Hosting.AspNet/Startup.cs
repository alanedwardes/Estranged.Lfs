using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Estranged.GitLfs.Api;
using Estranged.GitLfs.Storage;

namespace Estranged.GitLfs.Hosting.AspNet
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IBlobStore, DummyBlobStore>();
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
