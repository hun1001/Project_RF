using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : Node
{
    public SelectorNode(Node parent) : base(parent)
    {
    }

    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}