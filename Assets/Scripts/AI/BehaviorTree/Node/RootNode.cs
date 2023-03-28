using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : Node
{
    public override NodeStateType Execute()
    {
        return _children[0].Execute();
    }
}
