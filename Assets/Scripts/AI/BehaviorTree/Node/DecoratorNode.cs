using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoratorNode : Node
{
    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}