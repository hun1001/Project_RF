using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTreeBFSIterator
{
    private TechTree _techTree = null;

    private Queue<TechTreeNode> _nodeQueue = new Queue<TechTreeNode>();

    public TechTreeBFSIterator(TechTree techTree)
    {
        _techTree = techTree;

        _nodeQueue.Enqueue(_techTree.Root);
    }

    public bool IsSearching => _nodeQueue.Count > 0;

    public TechTreeNode GetNextNode()
    {
        var node = _nodeQueue.Dequeue();

        if(node.upChild != null)
        {
            _nodeQueue.Enqueue(node.upChild);
            OnEnqueueUpChildNode();
        }

        if(node.child != null)
        {
            _nodeQueue.Enqueue(node.child);
            OnEnqueueChildNode();
        }

        if(node.downChild != null)
        {
            _nodeQueue.Enqueue(node.downChild);
            OnEnqueueDownChildNode();
        }

        return node;
    }

    protected virtual void OnEnqueueUpChildNode()
    {

    }

    protected virtual void OnEnqueueChildNode()
    {
    }

    protected virtual void OnEnqueueDownChildNode()
    {
    }
}
