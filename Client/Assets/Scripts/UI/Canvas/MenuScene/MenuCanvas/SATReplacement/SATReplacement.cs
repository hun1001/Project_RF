using Addressable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SATReplacement : MonoBehaviour
{
    //[SerializeField]
    //private SATToggleTamplateHandle _satToggleTamplate = null;

    private void Awake()
    {
        SATSetting();
    }

    private void SATSetting()
    {
        var sats = AddressablesManager.Instance.GetLabelResources<TipSO>("Tip");

        foreach (var sat in sats)
        {
            Debug.Log(sat.name);
        }
    }
}
