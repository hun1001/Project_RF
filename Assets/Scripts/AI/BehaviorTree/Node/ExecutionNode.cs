using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionNode : Node
{
    private Action _action = null;

    public ExecutionNode(Action action) => _action = action;

    public override NodeStateType Execute()
    {
        _action?.Invoke();
        return NodeStateType.SUCCESS;
    }
}