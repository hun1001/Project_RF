using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree
{
    private RootNode _root;

    public BehaviorTree(RootNode root)
    {
        this._root = root;
    }

    public void Execute()
    {
        _root.Execute();
    }
}
