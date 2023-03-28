using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileNode : DecoratorNode
{
    public WhileNode(Func<bool> condition) : base(condition)
    {

    }

    public override NodeStateType Execute()
    {
        while (Condition())
        {
            foreach (var child in _children)
            {
                var state = child.Execute();

                if (state == NodeStateType.FAILURE)
                {
                    return NodeStateType.FAILURE;
                }
            }
        }
        return NodeStateType.SUCCESS;
    }
}