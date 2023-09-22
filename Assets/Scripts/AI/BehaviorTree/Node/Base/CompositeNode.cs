using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeNode : INode
{
    public virtual NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}
