using System;
using System.Collections.Generic;

[Serializable]
public class TechTree
{
    public TechTreeNode Root;

    public TechTree()
    {
        Root = new TechTreeNode();
    }

    public int GetMaxTier()
    {
        Queue<TechTreeNode> queue = new Queue<TechTreeNode>();
        queue.Enqueue(Root);
        int depth = 0;

        while (queue.Count > 0)
        {
            int levelSize = queue.Count;
            for (int i = 0; i < levelSize; i++)
            {
                TechTreeNode currentNode = queue.Dequeue();
                if (currentNode.hasUpChild)
                {
                    queue.Enqueue(currentNode.upChild);
                }
                if (currentNode.hasChild)
                {
                    queue.Enqueue(currentNode.child);
                }
                if (currentNode.hasDownChild)
                {
                    queue.Enqueue(currentNode.downChild);
                }
            }
            depth++;
        }

        return depth;
    }

    public int GetWidth()
    {
        Queue<TechTreeNode> queue = new Queue<TechTreeNode>();
        queue.Enqueue(Root);
        int width = 0;
        while (queue.Count > 0)
        {
            int levelSize = queue.Count;
            width = Math.Max(width, levelSize);
            for (int i = 0; i < levelSize; i++)
            {
                TechTreeNode currentNode = queue.Dequeue();
                if (currentNode.hasUpChild)
                {
                    queue.Enqueue(currentNode.upChild);
                }
                if (currentNode.hasChild)
                {
                    queue.Enqueue(currentNode.child);
                }
                if (currentNode.hasDownChild)
                {
                    queue.Enqueue(currentNode.downChild);
                }
            }
        }
        return width;
    }
}
