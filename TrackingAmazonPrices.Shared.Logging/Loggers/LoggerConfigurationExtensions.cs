using Serilog;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Transport;

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
        GraylogLoggerConfiguration grayLogConfig)
    {
        GraylogSinkOptions options = new()
        {
            HostnameOrAddress = grayLogConfig.Host,
            Port = grayLogConfig.Port,
            TransportType = TransportType.Udp,
            UseSsl = grayLogConfig.UseSsl,
            MinimumLogEventLevel = grayLogConfig.MinimumLevel
        };

        return grayLogConfig.Enabled
            ? loggerConfiguration.WriteTo.Graylog(options)
            : loggerConfiguration;
    }
}