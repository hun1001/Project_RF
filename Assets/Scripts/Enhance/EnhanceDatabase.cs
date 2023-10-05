using System.Collections;
using System.Collections.Generic;
using Addressable;
using UnityEngine;

public static class EnhanceDatabase
{
    private static List<ShellEnhance> _shellEnhanceList = null;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        _shellEnhanceList = new List<ShellEnhance>();

        var enhanceList = new List<BaseEnhance>();

        var enhances = AddressablesManager.Instance.GetLabelResourcesComponents<BaseEnhance>("Enhance");

        for (int i = 0; i < enhances.Count; ++i)
        {
            enhanceList.Add(enhances[i]);
        }

        for(int i = 0;i< enhances.Count; ++i)
        {
            var enhance = enhanceList[i] as ShellEnhance;
            if (enhance != null)
            {
                _shellEnhanceList.Add(enhance);
            }
        }


    }
}
