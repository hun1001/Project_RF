using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Addressable;
using UnityEngine;

public static class EnhanceDatabase
{
    private static List<BaseEnhance> _enhanceList = null;
    private static List<ShellEnhance> _shellEnhanceList = null;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        _enhanceList = GetEnhances();
        _shellEnhanceList = new List<ShellEnhance>();

        ClassifyEnhance(ref _shellEnhanceList);
    }

    private static List<BaseEnhance> GetEnhances()
    {
        return AddressablesManager.Instance.GetLabelResourcesComponents<BaseEnhance>("Enhance").ToList();
    }

    private static void ClassifyEnhance<T>(ref List<T> enhanceList) where T : BaseEnhance
    {
        for (int i = 0; i < _enhanceList.Count; ++i)
        {
            var enhance = _enhanceList[i] as T;
            if (enhance != null)
            {
                enhanceList.Add(enhance);
            }
        }
    }
}
