using FluentAssertions;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using TrackingAmazonPrices.Shared.Logging.Loggers;
using Xunit;

namespace TrackingAmazonPrices.Tests.Shared.Logging.Unit;

public class LoggerConfigurationExtensionsTests
{
    [Fact]
    public void AddConsoleLogger_Enabled_AddsConsoleLoggerInformationLevel()
    {
        LoggerConfiguration loggerConfiguration = new();
        ConsoleLoggerConfiguration consoleConfiguration
            = GetConsoleConfiguration(LogEventLevel.Information, true);

        var result = loggerConfiguration.AddConsoleLogger(consoleConfiguration);

        Logger logger = result.CreateLogger();

        var infoEnable = logger.IsEnabled(LogEventLevel.Information);
        var debugEnable = logger.IsEnabled(LogEventLevel.Debug);

        result.Should().NotBeNull();
        infoEnable.Should().BeTrue();
        debugEnable.Should().BeFalse();
        logger.Should().NotBeNull();
        logger.Should().BeAssignableTo<Logger>();
    }

    [Fact]
    public void AddConsoleLogger_Enabled_AddsConsoleLoggerErrorLevel()
    {
        LoggerConfiguration loggerConfiguration = new();
        ConsoleLoggerConfiguration consoleConfiguration
            = GetConsoleConfiguration(LogEventLevel.Error, true);

        var result = loggerConfiguration.AddConsoleLogger(consoleConfiguration);

        Logger logger = result.CreateLogger();

        var verboseLevel = logger.IsEnabled(LogEventLevel.Verbose);
        var errorLevel = logger.IsEnabled(LogEventLevel.Error);

        result.Should().NotBeNull();
        verboseLevel.Should().BeFalse();
        errorLevel.Should().BeTrue();
    }

    [Fact]
    public void AddGraylogLogger_Enabled_AddsGraylogLogger()
    {
        var loggerConfiguration = new LoggerConfiguration();
        GraylogLoggerConfiguration graylogLoggerConfiguration = GetGrayLogConfig();

        var result = loggerConfiguration.AddGraylogLogger(graylogLoggerConfiguration);

        result.Should().NotBeNull();

        var logger = result.CreateLogger();

        logger.Should().NotBeNull();
    }

    private static ConsoleLoggerConfiguration GetConsoleConfiguration(
        LogEventLevel level,
        bool enabled)
    {
        return new ConsoleLoggerConfiguration
        {
            Enabled = enabled,
            MinimumLevel = level
        };
    }

    private static GraylogLoggerConfiguration GetGrayLogConfig()
    {
        return new GraylogLoggerConfiguration
        {
            Enabled = true,
            Host = "localhost",
            Port = 12201,
            MinimumLevel = LogEventLevel.Information,
            UseSsl = false
        };
    }
}