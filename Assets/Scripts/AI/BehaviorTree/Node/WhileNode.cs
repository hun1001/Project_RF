using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileNode : DecoratorNode
{
    public WhileNode(Func<bool> condition) : base(condition)
    {

    }

    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}