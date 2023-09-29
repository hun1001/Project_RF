using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Addressable;
using Newtonsoft.Json;

public static class TechTreeInformationManager
{
    private static List<TechTree> _techTreeList = new List<TechTree>();
    public static List<TechTree> TechTreeList => _techTreeList;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        var temp = AddressablesManager.Instance.GetLabelResources<TextAsset>("TechTree");

        for (int i = 0; i < temp.Count; ++i)
        {
            var techTree = JsonConvert.DeserializeObject<TechTree>(temp[i].text);
            _techTreeList.Add(techTree);
        }
    }
}
