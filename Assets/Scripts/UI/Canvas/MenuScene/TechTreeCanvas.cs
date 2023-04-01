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
    
    [SerializeField]
    private Transform _techTreeContentTransform = null;

    private void Awake()
    {
        _countryDropdown.options.Clear();
        foreach (var techTreeSO in _techTree.TechTreeSO)
        {
            _countryDropdown.options.Add(new Dropdown.OptionData(techTreeSO.CountryType.ToString()));
        }
    }
}
