using Godot;
using System;

namespace utilities;

public static class NodeExtensions {

    public static bool TryFindParentNode<T>(this Node node, out T parentNode)
        where T : class
    {
        Node parent;
        
        if ((parent = node.GetParent()) != null)
        {
            if (parent is T)
            {
                parentNode = parent as T;
                return true;
            }
            else
            {
                return parent.TryFindParentNode(out parentNode);
            }
        }
        else 
        {
            parentNode = null;
            return false;
        }
    }
    
    public static T FindParentNodeIfNotSet<T>(this Node node, T value)
        where T : class
    {
        if (value is not null)
        {
            return value;
        }
        else
        {
            if(node.TryFindParentNode<T>(out var parentValue))
            {
                return parentValue;
            }
            else
            {
                throw new ArgumentException($"Could not find a suitable parent class {typeof(T).Name} for node {node.GetPath()}");
            }
        }
    }

    public static Node GetSceneRoot(this Node node)
    {
        return node.GetTree().Root.GetChild(0);
    }

    public static bool TryFindNodeInChildrenRecursively<T>(this Node node, out T value)
        where T : class
    {
        return node.TryFindNodeInChildrenRecursively(out value, (_) => true);
    }

    public static bool TryFindNodeInChildrenRecursively<T>(this Node node, out T value, Func<T, bool> filter)
        where T : class
    {
        if(node is T && filter(node as T))
        {
            value = node as T;
            return true;
        }

        foreach (var child in node.GetChildren())
        {
            if(child.TryFindNodeInChildrenRecursively(out value, filter))
            {
                return true;
            }
        }
        
        value = null;
        return false;
    } 

    public static bool TryFindNodeInScene<T>(this Node node, out T value)
        where T : class
    { 
        var root = node.GetSceneRoot();
        
        return root.TryFindNodeInChildrenRecursively(out value);
    }

    public static bool TryFindNodeInScene<T>(this Node node, out T value, Func<T, bool> filter)
        where T : class
    { 
        var root = node.GetSceneRoot();
        
        return root.TryFindNodeInChildrenRecursively(out value, filter);
    }
    
    public static void MoveToParent(this Node node, Node target)
    {
        var sourceTarget = node.GetParent();
        sourceTarget.RemoveChild(node);
        target.AddChild(node);
    }
    
    public static void ResetRotation<T>(this T node)
        where T : Node3D
    {
        Transform3D transform = node.Transform;
        transform.Basis = Basis.Identity;
        node.Transform = transform;
    }
}
