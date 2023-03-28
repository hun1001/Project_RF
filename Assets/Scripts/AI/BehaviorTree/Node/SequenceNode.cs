using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : Node
{
    public override NodeStateType Execute()
    {
        foreach (var child in _children)
        {
            var state = child.Execute();

            if (state == NodeStateType.FAILURE)
            {
                return NodeStateType.FAILURE;
            }
        }

        return NodeStateType.SUCCESS;
    }
}