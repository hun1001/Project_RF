using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TechTreeNode 
{
    public string tankAddress;

    public TechTreeNode upChildren;

    public TechTreeNode _child;

    public TechTreeNode downChildren;

    public TechTreeNode()
    {
        tankAddress = "";

        upChildren = null;
        _child = null;
        downChildren = null;
    }
}
