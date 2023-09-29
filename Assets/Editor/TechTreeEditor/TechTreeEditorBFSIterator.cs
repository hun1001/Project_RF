using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTreeEditorBFSIterator : TechTreeBFSIterator
{
    private Queue<Rect> _rectQueue = new Queue<Rect>();

    public TechTreeEditorBFSIterator(TechTree techTree, Rect rect) : base(techTree)
    {
        _rectQueue.Enqueue(rect);
    }

    public Rect GetNextRect()
    {
        return _rectQueue.Dequeue();
    }

    protected override void OnEnqueueUpChildNode()
    {
        var curretRect = _rectQueue.Peek();
        var nextRect = new Rect(curretRect.x + 140, curretRect.y - 60, curretRect.width, curretRect.height);

        _rectQueue.Enqueue(nextRect);
    }

    protected override void OnEnqueueChildNode()
    {
        var curretRect = _rectQueue.Peek();
        var nextRect = new Rect(curretRect.x + 140, curretRect.y, curretRect.width, curretRect.height);

        _rectQueue.Enqueue(nextRect);
    }

    protected override void OnEnqueueDownChildNode()
    {
        var curretRect = _rectQueue.Peek();
        var nextRect = new Rect(curretRect.x + 140, curretRect.y + 60, curretRect.width, curretRect.height);

        _rectQueue.Enqueue(nextRect);
    }
}
