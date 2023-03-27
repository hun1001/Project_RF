using System.Collections.Generic;

public abstract class Node
{
    protected List<Node> _children = new List<Node>();

    public Node() { }

    public abstract NodeStateType Execute();

    public void AddChild(Node child)
    {
        _children.Add(child);
    }

    public bool IsLeaf()
    {
        return _children.Count == 0;
    }

    public Node GetChildNode(int index)
    {
        return _children[index];
    }
}
