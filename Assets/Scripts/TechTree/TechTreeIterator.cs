using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTreeIterator
{
    private TechTree _techTree = null;

    private Queue<TechTreeNode> _nodeQueue = new Queue<TechTreeNode>();

    public TechTreeIterator(TechTree techTree)
    {
        _techTree = techTree;

        _nodeQueue.Enqueue(_techTree.Root);
    }

    public bool IsSearching => _nodeQueue.Count > 0;

    public TechTreeNode GetNextNode()
    {
        var node = _nodeQueue.Dequeue();

        if(node.upChildren != null)
        {
            _nodeQueue.Enqueue(node.upChildren);
        }

        if(node._child != null)
        {
            _nodeQueue.Enqueue(node._child);
        }

        if(node.downChildren != null)
        {
            _nodeQueue.Enqueue(node.downChildren);
        }

        return node;
    }
}
