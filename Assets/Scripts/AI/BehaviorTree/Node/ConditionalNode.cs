using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalNode : DecoratorNode
{
    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}
