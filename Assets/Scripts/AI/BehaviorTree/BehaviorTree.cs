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
        while (_nodeStack.Count > 0)
        {
            Node node = _nodeStack.Pop();
            NodeStateType nodeState = node.Execute();
            if (nodeState == NodeStateType.RUNNING)
            {
                _nodeStack.Push(node);
                foreach (Node child in node.Children)
                {
                    _nodeStack.Push(child);
                }
            }
            else if (nodeState == NodeStateType.FAILURE)
            {
                break;
            }
            else if (nodeState == NodeStateType.SUCCESS)
            {
                continue;
            }
        }
    }
}
