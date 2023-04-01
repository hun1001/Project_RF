using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TechTree : MonoBehaviour
{
    [SerializeField]
    private TechTreeSO[] _techTreeSO = null;
    public TechTreeSO[] TechTreeSO => _techTreeSO;
}
