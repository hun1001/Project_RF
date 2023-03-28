using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratorNode : INode
{
    public virtual NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}