using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FunWithAspNetCoreMiddlewares
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // This method is called only once on the application startup.
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<Startup>()
                    .ConfigureLogging((WebHostBuilderContext hostingContext, ILoggingBuilder loggingBuilder) =>
                    {
                        loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        loggingBuilder.AddConsole();
                        loggingBuilder.AddDebug();
                    });
                });
    }
}