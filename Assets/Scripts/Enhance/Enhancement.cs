using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhancement
{
    private Dictionary<ShellType, List<ShellEnhance>> _shellEnhanceDictionary = new();

    public void Init()
    {
        _shellEnhanceDictionary.Clear();
    }

    public void AddShellEnhance(ShellType type, ShellEnhance enhance)
    {
        if(!_shellEnhanceDictionary.ContainsKey(type))
        {
            _shellEnhanceDictionary.Add(type, new List<ShellEnhance>());
        }

        _shellEnhanceDictionary[type].Add(enhance);
    }

    public ShellEnhance[] GetShellEnhance(ShellType type)
    {
        return _shellEnhanceDictionary[type].ToArray();
    }
}
