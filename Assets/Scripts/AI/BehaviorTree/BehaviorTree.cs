using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree
{
    private RootNode _root;
    private Stack<Node> _nodeStack = new Stack<Node>();

    public BehaviorTree(RootNode root)
    {
        this._root = root;
    }

    public void Execute()
    {
        _nodeStack.Push(_root);
    }
}
