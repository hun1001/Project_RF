using System.Collections.Generic;

public abstract class Node
{
    private Node _parent = null;
    public Node Parent => _parent;

    protected List<Node> _children = new List<Node>();
    public List<Node> Children => _children;

    public Node(Node parent)
    {
        _parent = parent;
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
