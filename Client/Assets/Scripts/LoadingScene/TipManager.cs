using Addressable;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TipManager : MonoBehaviour
{
    [SerializeField]
    private Text _tipText = null;

    private List<TipSO> tipSOList = new List<TipSO>();
    private List<string> beforeTipText = new List<string>();

    private void Awake()
    {
        tipSOList = AddressablesManager.Instance.GetLabelResources<TipSO>("Tip").ToList();
    }

    private void Start() => SetRandomTipText();

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SetRandomTipText();
        }
    }

    private void SetRandomTipText()
    {
        _tipText.text = GetRandomTipText();
    }

    private string GetRandomTipText()
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
        Debug.Log("TipText: " + randomTipText);
        return randomTipText;
    }
}
