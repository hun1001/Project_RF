using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalNode : DecoratorNode
{
    public ConditionalNode(Func<bool> condition, INode child) : base(condition, child)
    {
    }

    public override NodeStateType Execute()
    {
        if (Condition() == true)
        {
            return _child.Execute();
        }
        else
        {
            return NodeStateType.FAILURE;
        }
    }
}
