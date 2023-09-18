using Serilog.Sinks.Graylog.Core.Transport;
using Serilog;
using Serilog.Sinks.Graylog;

namespace TrackingAmazonPrices.Shared.Logging.Loggers;

public static class LoggerConfigurationExtensions
{
    public static LoggerConfiguration AddConsoleLogger(
        this LoggerConfiguration loggerConfiguration,
        ConsoleLoggerConfiguration consoleLoggerConfiguration)
    {
        return consoleLoggerConfiguration.Enabled
            ? loggerConfiguration.WriteTo.Console(consoleLoggerConfiguration.MinimumLevel)
            : loggerConfiguration;
    }


    public static LoggerConfiguration AddGraylogLogger(
        this LoggerConfiguration loggerConfiguration,
        GraylogLoggerConfiguration graylogLoggerConfiguration)
    {
        GraylogSinkOptions options = new()
        {
            HostnameOrAddress = graylogLoggerConfiguration.Host,
            Port = graylogLoggerConfiguration.Port,
            TransportType = TransportType.Udp,
            UseSsl = false,
            MinimumLogEventLevel = graylogLoggerConfiguration.MinimumLevel
        };

        return graylogLoggerConfiguration.Enabled
            ? loggerConfiguration.WriteTo.Graylog(options)
            : loggerConfiguration;
    }

}
