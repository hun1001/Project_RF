using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TechTreeProgress
{
    public List<string> _tankProgressList;

    public TechTreeProgress()
    {
        _tankProgressList = new List<string>();
    }
}
