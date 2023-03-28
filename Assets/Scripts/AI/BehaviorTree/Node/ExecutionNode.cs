using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionNode : INode
{
    public NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}