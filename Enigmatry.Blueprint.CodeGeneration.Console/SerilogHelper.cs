using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;
using System.Reflection;

namespace Enigmatry.Blueprint.CodeGeneration.Console
{
    public static class SerilogHelper
    {
        public static void ConfigureSerilog()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        public static IServiceCollection AppSerilog(this IServiceCollection services)
        {
            services
                .AddLogging(builder =>
                {
                    builder.ClearProviders(); // kill default console log configuration added by CreateDefaultBuilder().
                    builder.AddSerilog();
                });

            return services;
        }
    }
}
