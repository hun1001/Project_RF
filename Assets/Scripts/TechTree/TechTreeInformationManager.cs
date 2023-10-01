using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Addressable;
using Newtonsoft.Json;

public static class TechTreeInformationManager
{
    private static List<TechTreeInformation> _techTreeInformationList = new List<TechTreeInformation>();
    public static List<TechTreeInformation> TechTreeInformationList => _techTreeInformationList;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        var temp = AddressablesManager.Instance.GetLabelResources<TextAsset>("TechTree");

        for (int i = 0; i < temp.Count; ++i)
        {
            var techTree = JsonConvert.DeserializeObject<TechTreeInformation>(temp[i].text);
            _techTreeInformationList.Add(techTree);
        }
    }
}
