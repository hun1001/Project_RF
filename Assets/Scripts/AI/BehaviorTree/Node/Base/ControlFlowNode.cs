using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlFlowNode : INode
{
    public bool Execute()
    {
        return true;
    }
}