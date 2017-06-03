using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Estranged.Lfs.Hosting.AspNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
