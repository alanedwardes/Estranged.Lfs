using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace Estranged.Lfs.Hosting.Lambda
{
    public class LocalEntryPoint
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
