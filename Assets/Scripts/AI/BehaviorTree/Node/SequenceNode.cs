using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : CompositeNode
{
    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}