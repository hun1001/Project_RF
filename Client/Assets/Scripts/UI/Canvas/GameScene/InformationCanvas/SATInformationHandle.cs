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
        (transform as RectTransform).SetAsLastSibling();
        gameObject.SetActive(true);

        _subArmament = sat;
        _satIconImage.sprite = _subArmament.Icon;

        sat.AddOnCoolingAction(OnCooling);

        StartCoroutine(nameof(FillImageValueUpdateCoroutine));
    }

    private IEnumerator FillImageValueUpdateCoroutine()
    {
        while (true)
        {
            _fillValueImage.fillAmount = _subArmament.CurretBeltCapacity / (float)_subArmament.GetSATSO().BeltCapacity;
            yield return null;
        }
    }

    private void OnCooling()
    {
        StartCoroutine(CoolingCoroutine());
    }

    private IEnumerator CoolingCoroutine()
    {
        StopCoroutine(nameof(FillImageValueUpdateCoroutine));
        _fillValueImage.fillAmount = 0f;
        _fillValueImage.color = new Color(1f, 0f, 0f, 0.12f);

        float time = 0f;

        while (time < _subArmament.GetSATSO().ReloadTime)
        {
            _fillValueImage.fillAmount = time / _subArmament.GetSATSO().ReloadTime;
            time += Time.deltaTime;
            yield return null;
        }

        _fillValueImage.fillAmount = 1f;
        _fillValueImage.color = new Color(1f, 1f, 1f, 0.12f);
        StartCoroutine(nameof(FillImageValueUpdateCoroutine));
    }
}
