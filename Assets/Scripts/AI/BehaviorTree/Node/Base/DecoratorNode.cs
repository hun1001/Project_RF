using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratorNode : Node
{
    private Func<bool> _condition;
    protected Func<bool> Condition => _condition;

    public DecoratorNode(Func<bool> condition, Node parent) : base(parent)
    {
        _condition = condition;
    }

    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}