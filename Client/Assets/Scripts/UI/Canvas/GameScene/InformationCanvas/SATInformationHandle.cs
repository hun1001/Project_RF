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

        sat.AddOnFireAction(OnFire);
        sat.AddOnReloadAction(OnReload);
    }

    private void OnFire()
    {
        _fillValueImage.fillAmount = (float)_subArmament.CurretBeltCapacity / _subArmament.GetSATSO().BeltCapacity;
    }

    private void OnReload()
    {
        StartCoroutine(ReloadImageFillCoroutine());
    }

    private IEnumerator ReloadImageFillCoroutine()
    {
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
    }
}
