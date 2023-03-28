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
        _nodeStack.Push(_root);
    }

    public void Execute()
    {
        while (_nodeStack.Count > 0)
        {
            Node node = _nodeStack.Pop();

            for (int i = 0; i < node.Children.Count; ++i)
            {
                _nodeStack.Push(node.Children[i]);
            }
        }
    }
}
