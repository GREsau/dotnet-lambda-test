using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace HelloWorld
{
    public static class Program
    {
        public static Task Main(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args);
            return ConfigureWebHost(builder)
                .Build()
                .RunAsync();
        }

        public static IWebHostBuilder ConfigureWebHost(IWebHostBuilder builder) =>
            builder.UseStartup<Startup>();
    }
}
