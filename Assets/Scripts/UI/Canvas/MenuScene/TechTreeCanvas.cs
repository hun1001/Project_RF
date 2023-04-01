using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechTreeCanvas : BaseCanvas
{
    [SerializeField]
    private TechTree _techTree = null;
    
    [SerializeField]
    private Dropdown _countryDropdown = null;
}
