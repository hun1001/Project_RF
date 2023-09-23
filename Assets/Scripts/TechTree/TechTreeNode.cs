using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TechTreeNode 
{
    public string tankAddress;

    public TechTreeNode upChild;

    public TechTreeNode child;

    public TechTreeNode downChild;

    public TechTreeNode()
    {
        tankAddress = "";

        upChild = null;
        child = null;
        downChild = null;
    }
}
