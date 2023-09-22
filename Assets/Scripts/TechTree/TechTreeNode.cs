using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTreeNode 
{
    public string tankAddress;
    public List<TechTreeNode> children;

    public TechTreeNode(string tankAddress)
    {
        this.tankAddress = tankAddress;
        children = new List<TechTreeNode>();
    }
}
