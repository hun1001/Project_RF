using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTreeCanvasBFSIterator : TechTreeBFSIterator
{
    private Queue<RectTransform> _rectTransformQueue = new Queue<RectTransform>();

    public TechTreeCanvasBFSIterator(TechTree techTree, RectTransform rectTransform) : base(techTree)
    {
        _rectTransformQueue.Enqueue(rectTransform);
    }

    public RectTransform GetNextRectTransform()
    {
        return _rectTransformQueue.Dequeue();
    }

    protected override void OnEnqueueUpChildNode()
    {

    }

    protected override void OnEnqueueChildNode()
    {

    }

    protected override void OnEnqueueDownChildNode()
    {

    }
}
