using System;
using System.Collections.Generic;
using UnityEngine;

public class TechTree
{
    private TechTreeNode root;
    public TechTreeNode Root => root;

    public TechTree()
    {
        this.root = new TechTreeNode();
    }

    public bool IsRoot(TechTreeNode node) => node == root;
}
