using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace HaberEcommerceSite
{
    public class Program
    {
        public static void Main(string[] arguments)
        {
            var host = BuildWebHost(arguments);

            host.Run();         
        }

        public static void SetupConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            // Remove the default configuration options.
            builder.Sources.Clear();

            // Set the configuration files for the project. In ascending order of importance.
            builder.AddJsonFile("Configuration.json", false, true)

                .AddEnvironmentVariables();
        }

        public static IWebHost BuildWebHost(string[] arguments)
        {
            return WebHost.CreateDefaultBuilder(arguments)

                // Set up the configuration for the project.
                .ConfigureAppConfiguration(SetupConfiguration)

                // Change the default wwwroot directory of the web host.
                .UseWebRoot("Web Root")

                // Set the startup class of the web host.
                .UseStartup<Startup>()

                // Build the web host with the given configuration.
                .Build();
        }
    }
}
