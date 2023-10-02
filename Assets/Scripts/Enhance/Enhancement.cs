using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhancement
{
    private Dictionary<ShellType, List<BaseEnhance>> _shellEnhanceDictionary = new();

    public void Init()
    {
        _shellEnhanceDictionary.Clear();
    }

    public void AddEnhance(EnhanceType enhanceType, BaseEnhance enhance)
    {

    }
}
