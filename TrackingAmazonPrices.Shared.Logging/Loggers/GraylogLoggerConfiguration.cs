using Serilog.Events;

namespace TrackingAmazonPrices.Shared.Logging.Loggers;

public class GraylogLoggerConfiguration
{
    public bool Enabled { get; set; } = false;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public LogEventLevel MinimumLevel { get; set; }
}