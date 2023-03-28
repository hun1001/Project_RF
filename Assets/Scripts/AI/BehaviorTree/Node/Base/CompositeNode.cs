using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeNode : Node
{
    public CompositeNode(Node parent) : base(parent)
    {
    }

    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}
