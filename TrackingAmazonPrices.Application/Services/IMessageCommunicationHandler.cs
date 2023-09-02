using TrackingAmazonPrices.Application.Handlers;

namespace TrackingAmazonPrices.Application.Services;

public interface IComunicationHandler
{
    IHandlerMessage StartComunication(IHandlerMessage handlerMessage);
}