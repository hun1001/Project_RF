using System.Collections.Generic;

public abstract class Node
{
    private NodeStateType _nodeState;
    private List<Node> _children = new List<Node>();

    public abstract NodeStateType Execute();
    public void AddChild(Node child)
    {
        _children.Add(child);
    }
}
