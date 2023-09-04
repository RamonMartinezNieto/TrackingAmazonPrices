using TrackingAmazonPrices.Application.Handlers;

namespace TrackingAmazonPrices.Application.Services;

public interface IComunicationHandler
{
    IMessageHandler StartComunication();
}