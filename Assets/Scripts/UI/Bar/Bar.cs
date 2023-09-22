using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bar : MonoBehaviour, IPoolReset
{
    [Header("Animation")]
    [SerializeField]
    private float _animationStartDelay = 0f;

    private bool _isSetting = false;

    [Header("UI")]
    [SerializeField]
    private Image _beforeValueImage = null;

    [SerializeField]
    private Image _valueImage = null;

    [SerializeField]
    private Text _valueText = null;

    private float _maxValue = 0;
    private float _currentValue = 0;

    private bool _isChangingBeforeBar = false;


    public void Setting(float max, float cur = -1)
    {
        if (_isSetting == true)
        {
            return;
        }

        _maxValue = max;
        if (cur == -1)
        {
            _currentValue = _maxValue;
        }

        UpdateValueText();
        _isSetting = true;
    }

    public void PoolObjectReset()
    {
        _isSetting = false;
        _valueImage.fillAmount = 1f;
        _beforeValueImage.fillAmount = 1f;
    }

    public void ChangeValue(float value)
    {
        _currentValue += value;

        if (_currentValue > _maxValue) _currentValue = _maxValue;
        if (_currentValue < 0) _currentValue = 0;

        _valueImage.fillAmount = _currentValue / _maxValue;

        UpdateValueText();

        StartCoroutine(nameof(ChangeBeforeBarCoroutine));
    }

    protected virtual void UpdateValueText()
    {
        _valueText.text = string.Format("{0:N0} HP", _currentValue);
    }

    private IEnumerator ChangeBeforeBarCoroutine()
    {
        if (_isChangingBeforeBar == true)
        {
            yield break;
        }

        _isChangingBeforeBar = true;
        yield return new WaitForSeconds(_animationStartDelay);

        _beforeValueImage.fillAmount = _valueImage.fillAmount;
        _isChangingBeforeBar = false;
    }
}
