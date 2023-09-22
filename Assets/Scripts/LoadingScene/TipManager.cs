using Addressable;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class TipManager : MonoSingleton<TipManager>
{
    private List<TipSO> tipSOList = new List<TipSO>();
    private List<string> beforeTipText = new List<string>();

    private void Awake()
    {
        tipSOList = AddressablesManager.Instance.GetLabelResources<TipSO>("Tip").ToList();
    }

    public string GetRandomTipText()
    {
        string randomTipText = tipSOList[Random.Range(0, tipSOList.Count)].tipText;
        while (beforeTipText.Contains(randomTipText))
        {
            randomTipText = tipSOList[Random.Range(0, tipSOList.Count)].tipText;
        }
        beforeTipText.Add(randomTipText);
        if (beforeTipText.Count > 5)
        {
            beforeTipText.RemoveAt(0);
        }

        return randomTipText;
    }
}
