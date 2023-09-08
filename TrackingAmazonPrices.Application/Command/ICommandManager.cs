using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Application.Command;

public interface ICommandManager
{
    bool IsCommand(string command);

    ICommand GetCommand(string messageCommand);

    ICommand GetNextCommand(Steps? step);

    ICommand NullCommand();
}