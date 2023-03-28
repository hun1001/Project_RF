using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalNode : DecoratorNode
{
    public ConditionalNode(Func<bool> condition) : base(condition)
    {

    }

    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}
