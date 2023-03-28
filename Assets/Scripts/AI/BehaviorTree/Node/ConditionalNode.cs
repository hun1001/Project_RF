using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalNode : DecoratorNode
{
    public ConditionalNode(Func<bool> condition, Node parent) : base(condition, parent)
    {

    }

    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}
