using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : CompositeNode
{
    private readonly List<INode> _children = new List<INode>();

    public SequenceNode(params INode[] children)
    {
        _children.AddRange(children);
    }

    public override NodeStateType Execute()
    {
        foreach (var child in _children)
        {
            if (child.Execute() == NodeStateType.FAILURE)
                return NodeStateType.FAILURE;
        }
        return NodeStateType.SUCCESS;
    }
}