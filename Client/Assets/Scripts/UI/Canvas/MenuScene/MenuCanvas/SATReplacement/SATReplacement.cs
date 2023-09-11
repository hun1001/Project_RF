using Addressable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SATReplacement : MonoBehaviour
{
    [SerializeField]
    private Transform _satTransform = null;

    [SerializeField]
    private SATToggleTamplateHandle _satToggleTamplate = null;

    private void Awake()
    {
        SATSetting();
    }

    private void SATSetting()
    {
        var satList = AddressablesManager.Instance.GetLabelResourcesComponents<BaseSubArmament>("SAT");

        foreach (var sat in satList)
        {
            var satToggle = Instantiate(_satToggleTamplate, _satTransform);
            satToggle.SetText(sat.name);
            satToggle.gameObject.SetActive(true);
        }
    }

    public bool ActiveSelf => gameObject.activeSelf;
    public void SetActive(bool active) => gameObject.SetActive(active);
}
