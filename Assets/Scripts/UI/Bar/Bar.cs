using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour, Pool.IPoolReset
{
    [SerializeField]
    private float _animationStartDelay = 0f;

    [SerializeField]
    private float _animationNextMoveDelay = 0f;

    private bool _isSetting = false;

    private Image _beforeValueImage = null;
    private Image _valueImage = null;
    private Text _valueText = null;

    private float _maxValue = 0;
    private float _currentValue = 0;

    private bool _isChangingBeforeBar = false;

    private void Awake()
    {
        _beforeValueImage = transform.GetChild(0).GetComponent<Image>();
        _valueImage = transform.GetChild(1).GetComponent<Image>();
        _valueText = transform.GetChild(2).GetComponent<Text>();
    }

    public void Setting(float max, float cur = -1)
    {
        if (_isSetting == true)
        {
            return;
        }

        _maxValue = max;
        if (cur < 0)
        {
            _currentValue = _maxValue;
        }
        _isSetting = true;
    }

    public void PoolObjectReset()
    {
        _isSetting = false;
    }

    public void ChangeValue(float value)
    {
        _currentValue += value;
        _valueImage.fillAmount = _currentValue / _maxValue;

        StartCoroutine(nameof(ChangeBeforeBarCoroutine));
    }

    private IEnumerator ChangeBeforeBarCoroutine()
    {
        if (_isChangingBeforeBar == true)
        {
            yield break;
        }

        _isChangingBeforeBar = true;
        yield return new WaitForSeconds(_animationStartDelay);
        while (_beforeValueImage.fillAmount > _valueImage.fillAmount)
        {
            _beforeValueImage.fillAmount -= 0.001f;
            yield return new WaitForSeconds(_animationNextMoveDelay);
        }
        _isChangingBeforeBar = false;
    }
}
