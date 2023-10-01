using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTreeEditorBFSIterator : TechTreeBFSIterator
{
    private Queue<Rect> _rectQueue = new Queue<Rect>();

    private float _maxX = 0;
    private float _maxY = 0;

    public float MaxX => _maxX;
    public float MaxY => _maxY;

    public TechTreeEditorBFSIterator(TechTree techTree, Rect rect) : base(techTree)
    {
        _maxX = rect.x;
        _maxY = rect.y;

        _rectQueue.Enqueue(rect);
    }

    public Rect GetNextRect()
    {
        return _rectQueue.Dequeue();
    }

    protected override void OnEnqueueUpChildNode()
    {
        var curretRect = _rectQueue.Peek();

        float newX = curretRect.x + 140;
        float newY = curretRect.y - 60;

        var nextRect = new Rect(newX, newY, curretRect.width, curretRect.height);

        _maxX = Mathf.Max(newX, _maxX);
        _maxY = Mathf.Max(newY, _maxY);

        _rectQueue.Enqueue(nextRect);
    }

    protected override void OnEnqueueChildNode()
    {
        var curretRect = _rectQueue.Peek();

        float newX = curretRect.x + 140;

        var nextRect = new Rect(newX, curretRect.y, curretRect.width, curretRect.height);

        _maxX = Mathf.Max(newX, _maxX);

        _rectQueue.Enqueue(nextRect);
    }

    protected override void OnEnqueueDownChildNode()
    {
        var curretRect = _rectQueue.Peek();

        float newX = curretRect.x + 140;
        float newY = curretRect.y + 60;

        var nextRect = new Rect(curretRect.x + 140, curretRect.y + 60, curretRect.width, curretRect.height);

        _maxX = Mathf.Max(newX, _maxX);
        _maxY = Mathf.Max(newY, _maxY);

        _rectQueue.Enqueue(nextRect);
    }
}
