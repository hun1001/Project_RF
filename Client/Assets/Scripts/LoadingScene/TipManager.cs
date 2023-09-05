using Addressable;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TipManager : MonoBehaviour
{
    [SerializeField]
    private Text _tipText = null;

    private List<TipSO> tipSOList = new List<TipSO>();

    private void Awake()
    {
        tipSOList = AddressablesManager.Instance.GetLabelResources<TipSO>("Tip").ToList<TipSO>();
    }

    private void Start()
    {
        _tipText.text = tipSOList[Random.Range(0, tipSOList.Count)].tipText;
    }
}
