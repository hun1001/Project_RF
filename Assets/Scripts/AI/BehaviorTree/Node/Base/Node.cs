using System.Collections.Generic;

public abstract class Node
{
    protected List<Node> _children = new List<Node>();
    public List<Node> Children => _children;

    public Node()
    {

    }

    public abstract NodeStateType Execute();

    public void AddChild(Node child)
    {
        _children.Add(child);
    }

    public bool IsLeaf()
    {
        return _children.Count == 0;
    }
}
