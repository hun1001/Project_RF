using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoolDownBarHandle : MonoBehaviour
{
    [SerializeField]
    private Image _coolDownBar = null;
    
    [SerializeField]
    private TMP_Text _coolTimeText = null;

    private float _maxCoolTime = 0;
    private float _coolTime = 0;

    public void SetCoolDown(float coolTime)
    {
        _maxCoolTime = _coolTime = coolTime;

        StartCoroutine(CoolDownCoroutine());
    }

    private IEnumerator CoolDownCoroutine()
    {
        do
        {
            _coolTimeText.text = _coolTime.ToString("F1") + "s";
            _coolDownBar.fillAmount = _coolTime / _maxCoolTime;

            _coolTime -= Time.deltaTime;

            yield return null;
        } while (_coolTime > 0);

        _coolTimeText.text = "";
        _coolDownBar.fillAmount = 0;
    }
}
