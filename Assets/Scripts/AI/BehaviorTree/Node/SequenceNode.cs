using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : Node
{
    public SequenceNode(Node parent) : base(parent)
    {
    }

    public override NodeStateType Execute()
    {


        return NodeStateType.SUCCESS;
    }
}