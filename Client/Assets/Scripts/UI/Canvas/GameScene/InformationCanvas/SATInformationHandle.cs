using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SATInformationHandle : MonoBehaviour
{
    [SerializeField]
    private Image _satIconImage = null;

    [SerializeField]
    private Image _fillValueImage = null;

    private BaseSubArmament _subArmament = null;

    public void Setting(BaseSubArmament sat)
    {
        gameObject.SetActive(true);
        _subArmament = sat;

        _satIconImage.sprite = _subArmament.Icon;
    }
}
