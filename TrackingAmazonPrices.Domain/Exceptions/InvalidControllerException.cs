using System;

namespace TrackingAmazonPrices.Domain.Exceptions;

public class InvalidControllerException : Exception
{
    private const string ExceptionMissingController = "Controller Message is not defined, call method SetControllerMessage";

    public InvalidControllerException() : base(ExceptionMissingController)
    {
    }
}
