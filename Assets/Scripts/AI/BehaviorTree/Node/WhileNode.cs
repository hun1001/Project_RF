using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileNode : Node
{
    public WhileNode(Node parent) : base(parent)
    {
    }

    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}