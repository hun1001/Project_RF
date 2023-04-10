using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionNode : INode
{
    private readonly Action _action;

    public ExecutionNode(Action action)
    {
        _action = action;
    }

    public NodeStateType Execute()
    {
        _action?.Invoke();
        return NodeStateType.SUCCESS;
    }
}