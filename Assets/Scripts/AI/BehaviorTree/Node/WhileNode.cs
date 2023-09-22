using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileNode : DecoratorNode
{
    public WhileNode(Func<bool> condition, INode child) : base(condition, child)
    {
    }

    public override NodeStateType Execute()
    {
        if (Condition())
        {
            return _child.Execute();
        }
        else
        {
            return NodeStateType.FAILURE;
        }
    }
}