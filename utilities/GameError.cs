
using System;
using CSharpFunctionalExtensions;

namespace utilities;

public class GameError : Exception, ICombine
{
    public GameError() : base()
    {
    }

    public GameError(string message) : base(message)
    {
    }

    public GameError(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ICombine Combine(ICombine value)
    {
        var other = value as GameError;

        var exceptions = new Exception[]
        {
            this,
            other
        };

        return new GameError("Multiple Errors Ocurred", new AggregateException(exceptions));
    }
}