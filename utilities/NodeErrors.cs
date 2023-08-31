using System;
using CSharpFunctionalExtensions;
using Godot;

namespace utilities;

public class NodeError : Exception, ICombine
{
    public NodeError() : base()
    {
    }

    public NodeError(string message) : base(message)
    {
    }

    public NodeError(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static NodeError AttemptedToExecuteOnNullNode<T>(string argumentName)
    {
        return new NodeError($"Attempted to call a function on null-node argument {argumentName}");
    }

    public static NodeError NodeIsNull<T>(Maybe<string> argumentName)
    {
        if (argumentName.HasValue)
        {
            return new NodeError($"{argumentName} is null when it should not be");
        }
        else
        {
            return new NodeError($"Node is null when it should not be");
        }
    }

    public static NodeError PropertyIsNotSetUsingEditor<T>(string argumentName)
    {
        return new NodeError($"Node {argumentName} of type {typeof(T).Name} was not set using editor even though it should have been");
    }

    public ICombine Combine(ICombine value)
    {
        var ohter = value as NodeError;

        var exceptions = new Exception[]
        {
            this,
            ohter
        };

        return new NodeError("Multiple Errors Ocurred", new AggregateException(exceptions));
    }
}

public class NodeSearchError : NodeError
{
    public NodeSearchError() : base()
    {
    }

    public NodeSearchError(string message) : base(message)
    {
    }

    public NodeSearchError(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static NodeError CouldNotFindNodeWhenSearching<T>()
    {
        return new NodeSearchError($"Could not locate the node of type {typeof(T).Name}");
    }
}

public class NoParentError : NodeSearchError
{
    public NoParentError() : base()
    {
    }

    public NoParentError(string message) : base(message)
    {
    }

    public NoParentError(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static NodeError CouldNotLocateParent(Node node)
    {
        return new NoParentError($"Could not locate parent of {node.GetPath()}");
    }
}

public class CouldNotCastToRequiredTypeError : NodeSearchError
{
    public CouldNotCastToRequiredTypeError() : base()
    {
    }

    public CouldNotCastToRequiredTypeError(string message) : base(message)
    {
    }

    public CouldNotCastToRequiredTypeError(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static NodeError CouldNotCastTo<T>(Node node)
    {
        return new CouldNotCastToRequiredTypeError($"Could not cast found node {node.GetPath()} " +
                $"of type {node.GetType()} to requested type {typeof(T).Name}");
    }
}
