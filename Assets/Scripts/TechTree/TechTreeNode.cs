using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTreeNode 
{
    public string tankAddress;

    public List<TechTreeNode> upChildren;

    public TechTreeNode _child;

    public List<TechTreeNode> downChildren;

    public TechTreeNode()
    {
        tankAddress = "";

        upChildren = new List<TechTreeNode>();
        _child = null;
        downChildren = new List<TechTreeNode>();
    }
}
