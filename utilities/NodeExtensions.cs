using CSharpFunctionalExtensions;
using Godot;
using System;

namespace utilities;

public static partial class NodeExtensions
{
    public static Result<T, NodeError> GetSceneRoot<T>(this T node)
        where T : Node
    {
        var nodeResult = node.ToNodeResult();

        return nodeResult.GetSceneRoot();
    }

    public static Result<T, NodeError> GetSceneRoot<T>(this Result<T, NodeError> node)
        where T : Node
    {
        var rootWindow = node.Bind(n => n.GetTree().Root.ToNodeResult());

        var sceneRoot = rootWindow.Bind(sr => sr.GetChild<T>(0).ToNodeResult());

        return sceneRoot;
    }

    public static Result<TResult, NodeError> FindNodeInChildrenRecursively<TResult>(this Node parent)
        where TResult : Node
    {
        var nodeResult = parent.ToNodeResult();

        return nodeResult.FindNodeInChildrenRecursively<TResult>((_) => true);
    }


    public static Result<TResult, NodeError> FindNodeInChildrenRecursively<TResult>(this Result<Node, NodeError> parent)
        where TResult : Node
    {
        return parent.FindNodeInChildrenRecursively<TResult>((_) => true);
    }

    public static Result<TResult, NodeError> FindNodeInChildrenRecursively<TResult>(this Result<Node, NodeError> parent, Func<TResult, bool> filter)
        where TResult : Node
    {
        if (parent.IsFailure)
        {
            return Result.Failure<TResult, NodeError>(parent.Error);
        }

        if (parent.Value is TResult && filter(parent.Value as TResult))
        {
            return Result.Success<TResult, NodeError>(parent.Value as TResult);
        }

        foreach (var child in parent.Value.GetChildren())
        {
            var childResult = child.ToNodeResult().FindNodeInChildrenRecursively(filter);

            if (childResult.IsSuccess)
            {
                return childResult;
            }
        }

        return NodeSearchError.CouldNotFindNodeWhenSearching<TResult>();
    }

    public static Result<T, NodeError> TryFindNodeInScene<T>(this Node node)
        where T : Node
    {
        var root = node.ToNodeResult().GetSceneRoot();

        return root.TryFindNodeInScene<T>();
    }

    public static Result<T, NodeError> TryFindNodeInScene<T>(this Result<Node, NodeError> node)
        where T : Node
    {
        return node.FindNodeInChildrenRecursively<T>();
    }

    public static UnitResult<NodeError> MoveToParent(this Node node, Node target)
    {
        var targetResult = target.ToNodeResult(nameof(target));

        if (targetResult.IsFailure)
        {
            return targetResult;
        }

        var nodeResult = node.ToNodeResult(nameof(node));
        var nodeParent = nodeResult.Bind(n => n.ToNodeResult().GetParentSafe<Node>());

        if (nodeParent.IsFailure)
        {
            return nodeParent;
        }

        nodeParent.Value.RemoveChild(nodeResult.Value);
        targetResult.Value.AddChild(nodeResult.Value);

        return UnitResult.Success<NodeError>();
    }

    public static UnitResult<NodeError> ResetRotation<T>(this T node)
        where T : Node3D
    {
        var nodeResult = node.ToNodeResult(nameof(node));

        return ResetRotation(nodeResult);
    }

    private static UnitResult<NodeError> ResetRotation<T>(Result<T, NodeError> nodeResult)
        where T : Node3D
    {
        var transformResult = nodeResult.Map(n => n.Transform);

        if (transformResult.TryGetValue(out var transform))
        {
            transform.Basis = Basis.Identity;
            nodeResult.Value.Transform = transform;
        }

        return transformResult;
    }

    public static Result<TParent, NodeError> GetParentSafe<TParent>(this Node node)
        where TParent : Node
    {
        return node.ToNodeResult().GetParentSafe<TParent>();
    }

    public static Result<TParent, NodeError> GetParentSafe<TParent>(this Maybe<Node> node)
        where TParent : Node
    {
        return node.ToNodeResult().GetParentSafe<TParent>();
    }

    public static Result<TParent, NodeError> GetParentSafe<TParent>(this Result<Node, NodeError> nodeResult)
        where TParent : Node
    {
        var parent = nodeResult.Bind(n =>
            n.GetParent().ToResult(NoParentError.CouldNotLocateParent(n)));

        var castParent = parent.Bind(p =>
            (p as TParent).ToResult(CouldNotCastToRequiredTypeError.CouldNotCastTo<Node>(p)));

        return castParent;
    }

    public static Result<T, NodeError> CompensateFromChildren<T>(this Result<T, NodeError> nodeResult, Node parent)
        where T : Node
    {
        if (nodeResult.IsFailure)
        {
            return parent.FindNodeInChildrenRecursively<T>();
        }

        return nodeResult;
    }

    public static Result<T, NodeError> CompensateFromParent<T>(this T node, Node child)
        where T : Node
    {
        var nodeResult = node.ToNodeResult();

        return nodeResult.CompensateFromParent(child);
    }

    public static Result<T, NodeError> CompensateFromParent<T>(this Result<T, NodeError> nodeResult, Node child)
        where T : Node
    {
        if (nodeResult.IsFailure)
        {
            return child.ToNodeResult().GetParentSafe<T>();
        }

        return nodeResult;
    }
}

public static class ResultExtensions
{
    public static Result<T, NodeError> ToNodeResult<T>(this T node, Maybe<string> attributeName = default)
    {
        return Maybe.From(node).ToNodeResult(attributeName);
    }

    public static Result<T, NodeError> ToNodeResult<T>(this Maybe<T> node, Maybe<string> attributeName = default)
    {
        return node.ToResult(error: NodeError.NodeIsNull<Node>(attributeName));
    }

    public static T AssertEditorPropertySet<T>(this T property, string attributeName)
    {
        var maybe = Maybe.From(property);

        return maybe.AssertEditorPropertySet(attributeName);
    }

    public static T AssertEditorPropertySet<T>(this Maybe<T> property, string attributeName)
    {
        return property
            .ToResult(error: NodeError.PropertyIsNotSetUsingEditor<T>(attributeName))
            .ThrowIfValueNotSet();
    }

    public static T ThrowIfValueNotSet<T, E>(this Result<T, E> result)
    {
        var value = result.Value;

        return value;
    }
}