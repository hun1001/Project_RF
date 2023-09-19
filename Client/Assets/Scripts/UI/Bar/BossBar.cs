using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    [SerializeField]
    private Image _image = null;

    [SerializeField]
    private Image _valueImage = null;

    private float _maxValue = 0;
    private float _currentValue = 0;

    public void Setting(float max, float cur = -1)
    {
        _maxValue = max;
        if (cur == -1)
        {
            _currentValue = _maxValue;
        }

        _valueImage.fillAmount = 1;

        _valueImage.enabled = true;
        _image.enabled = true;
    }

    public void ChangeValue(float value)
    {
        _currentValue += value;
        _currentValue = Mathf.Clamp(_currentValue, 0, _maxValue);
        _valueImage.fillAmount = _currentValue / _maxValue;
    }

    public void Hide()
    {
        _valueImage.enabled = false;
        _image.enabled = false;
    }
}
