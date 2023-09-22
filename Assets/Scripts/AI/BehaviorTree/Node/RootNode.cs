using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : INode
{
    private readonly INode _child = null;

    public RootNode(INode child) => _child = child;
    
    public NodeStateType Execute()
    {
        return _child.Execute();
    }
}
