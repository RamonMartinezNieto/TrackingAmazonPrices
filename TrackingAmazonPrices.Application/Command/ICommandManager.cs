namespace TrackingAmazonPrices.Application.Command;

public interface ICommandManager
{
    bool IsCommand(string command);

    ICommand GetCommand(string messageCommand);

    ICommand NullCommand();
}