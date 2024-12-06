using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Enigmatry.Entry.CodeGeneration.Tools;

public static class SerilogHelper
{
    public static void ConfigureSerilog()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(AppContext.BaseDirectory)!)
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
