using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
{
    private readonly List<INode> _children = new List<INode>();

    public SelectorNode(params INode[] children)
    {
        _children.AddRange(children);
    }

    public override NodeStateType Execute()
    {
        foreach (var child in _children)
        {
            if (child.Execute() == NodeStateType.SUCCESS)
                return NodeStateType.SUCCESS;
        }
        return NodeStateType.FAILURE;
    }
}