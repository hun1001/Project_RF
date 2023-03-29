using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratorNode : INode
{
    private readonly Func<bool> _condition;
    protected INode _child;

    public DecoratorNode(Func<bool> condition, INode child)
    {
        _child = child;
        _condition = condition;
    }

    public virtual NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }

    protected virtual bool Condition()
    {
        return _condition?.Invoke() == true;
    }
}