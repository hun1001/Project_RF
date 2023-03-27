using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : Node
{
    public override NodeStateType Execute()
    {
        return NodeStateType.SUCCESS;
    }
}
