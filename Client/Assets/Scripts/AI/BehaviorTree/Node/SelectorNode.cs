using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
{
    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}