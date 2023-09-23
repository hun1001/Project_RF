using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TechTreeNode 
{
    public string tankAddress;

    public TechTreeNode upChild;
    public bool hasUpChild = false;

    public TechTreeNode child;
    public bool hasChild = false;

    public TechTreeNode downChild;
    public bool hasDownChild = false;

    public TechTreeNode()
    {
        tankAddress = "";

        upChild = null;
        child = null;
        downChild = null;
    }
}
