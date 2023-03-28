using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlFlowNode : Node
{
    public ControlFlowNode(Node parent) : base(parent)
    {
    }

    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}