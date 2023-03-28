using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelNode : Node
{
    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}