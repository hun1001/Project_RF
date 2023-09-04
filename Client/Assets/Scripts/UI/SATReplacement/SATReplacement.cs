using Addressable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SATReplacement : MonoBehaviour
{
    [SerializeField]
    private SATToggleTamplateHandle _satToggleTamplate = null;

    private void Awake()
    {
        SATSetting();
    }

    private void SATSetting()
    {
        var sats = AddressablesManager.Instance.GetLabelResources<GameObject>("SAT");

        foreach (var sat in sats)
        {
            var satToggle = Instantiate(_satToggleTamplate, transform);
            satToggle.gameObject.SetActive(true);

            satToggle.SetText(sat.ToString());
        }
    }
}