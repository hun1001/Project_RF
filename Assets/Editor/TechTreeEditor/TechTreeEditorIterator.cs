using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTreeEditorIterator : TechTreeIterator
{
    private Queue<Rect> _rectQueue = new Queue<Rect>();

    public TechTreeEditorIterator(TechTree techTree, Rect rect) : base(techTree)
    {
        _rectQueue.Enqueue(rect);
    }

    public Rect GetNextRect()
    {
        return _rectQueue.Dequeue();
    }
}
