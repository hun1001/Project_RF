using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Addressable;

public static class TechTreeDataManager
{
    private static List<TechTree> _techTreeList = new List<TechTree>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        var json = AddressablesManager.Instance.GetLabelResources<TextAsset>("TechTree");
        Debug.Log(json);
    }
}
