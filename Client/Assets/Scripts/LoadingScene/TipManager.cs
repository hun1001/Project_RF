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
        _tipText.text = tipSOList[Random.Range(0, tipSOList.Count)].tipText;
    }
}
