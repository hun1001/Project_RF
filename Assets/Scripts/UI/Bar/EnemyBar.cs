using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBar : MonoBehaviour, IPoolReset
{
    private readonly static Vector3 _offset = Vector3.up * 3.5f + Vector3.back;

    [SerializeField]
    private Canvas _canvas = null;

    [SerializeField]
    private Image _valueImage = null;

    private float _maxValue = 0;
    private float _currentValue = 0;

    private float _showTime = 0f;

    public void Show()
    {
        _showTime = 10;
    }

    public void Setting(float max, float cur = -1)
    {
        _maxValue = max;
        if (cur == -1)
        {
            _currentValue = _maxValue;
        }

        _valueImage.fillAmount = 1;

        _canvas.enabled = false;
    }

    public void ChangeValue(float value)
    {
        _currentValue += value;
        _currentValue = Mathf.Clamp(_currentValue, 0, _maxValue);
        _valueImage.fillAmount = _currentValue / _maxValue;
    }

    private void Update()
    {
        _canvas.enabled = _showTime > 0;
        _showTime -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        transform.position = transform.parent.position + _offset;
    }

    public void PoolObjectReset()
    {
        _maxValue = 0;
        _currentValue = 0;
        _valueImage.fillAmount = 1f;
    }
}
