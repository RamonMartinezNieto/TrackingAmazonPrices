using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using TrackingAmazonPrices.Shared.Logging.Loggers;

namespace TrackingAmazonPrices.Shared.Logging;

[ExcludeFromCodeCoverage]
public static class ConfigureLogger
{
    public static IHostBuilder ConfigureSerilog(this IHostBuilder builder)
        => builder.UseSerilog((context, loggerConfiguration)
            => ConfigureSerilogLogger(loggerConfiguration, context.Configuration));

    private static LoggerConfiguration ConfigureSerilogLogger(
        LoggerConfiguration loggerConfiguration,
        IConfiguration configuration)
    {
        GraylogLoggerConfiguration graylogLogger = new ();
        configuration.GetSection("Logging:Graylog").Bind(graylogLogger);

        ConsoleLoggerConfiguration consoleLogger = new ();
        configuration.GetSection("Logging:Console").Bind(consoleLogger);

        return loggerConfiguration
                .AddConsoleLogger(consoleLogger)
                .AddGraylogLogger(graylogLogger);
    }
}
