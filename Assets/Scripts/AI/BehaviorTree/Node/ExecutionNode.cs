using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionNode : Node
{
    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}