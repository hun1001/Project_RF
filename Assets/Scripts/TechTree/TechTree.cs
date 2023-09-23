using System;

[Serializable]
public class TechTree
{
    public TechTreeNode Root;
    public CountryType Country;

    public TechTree()
    {
        this.Root = new TechTreeNode();
    }

    public bool IsRoot(TechTreeNode node) => node == Root;
}
