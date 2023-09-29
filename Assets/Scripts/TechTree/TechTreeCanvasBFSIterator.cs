using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TechTreeCanvasBFSIterator : TechTreeBFSIterator
{
    // Iterator�� ���� �� ���� ©�� ������ ������;; �ϴ� �����̽����� �Ƿ��̽������� �𸣰�����(�Ƹ� 90%�� �Ƿ� �̽�) ���� �������°� ������ �̷��.

    private Queue<Vector2> _rectTransformQueue = new Queue<Vector2>();
    private Queue<int> _tierQueue = new Queue<int>();

    public TechTreeCanvasBFSIterator(TechTree techTree, Vector2 position) : base(techTree)
    {
        _rectTransformQueue.Enqueue(position);
        _tierQueue.Enqueue(0);
    }

    public Vector2 GetNextPosition()
    {
        return _rectTransformQueue.Dequeue();
    }

    public int GetNextTier()
    {
        return _tierQueue.Dequeue();
    }

    protected override void OnEnqueueUpChildNode()
    {
        Vector2 curretRectTransform = _rectTransformQueue.Peek();
        Vector2 nextRectTransform = new Vector2(curretRectTransform.x + 280, curretRectTransform.y + 100);

        _tierQueue.Enqueue(_tierQueue.Peek() + 1);
        _rectTransformQueue.Enqueue(nextRectTransform);
    }

    protected override void OnEnqueueChildNode()
    {
        Vector2 curretRectTransform = _rectTransformQueue.Peek();
        Vector2 nextRectTransform = new Vector2(curretRectTransform.x + 280, curretRectTransform.y);

        _tierQueue.Enqueue(_tierQueue.Peek() + 1);
        _rectTransformQueue.Enqueue(nextRectTransform);
    }

    protected override void OnEnqueueDownChildNode()
    {
        Vector2 curretRectTransform = _rectTransformQueue.Peek();
        Vector2 nextRectTransform = new Vector2(curretRectTransform.x + 280, curretRectTransform.y - 100);
        
        _tierQueue.Enqueue(_tierQueue.Peek() + 1);
        _rectTransformQueue.Enqueue(nextRectTransform);
    }
}
