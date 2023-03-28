using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelNode : Node
{
    public ParallelNode(Node parent) : base(parent)
    {
    }

    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}