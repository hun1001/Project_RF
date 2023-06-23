using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyBar : Bar
{
    [SerializeField]
    private TextMeshPro _worldSpaceValueText = null;

    private static Vector3 _offset = Vector3.down * 3.5f + Vector3.back;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        transform.position = transform.parent.position + _offset;
    }

    protected override void UpdateValueText()
    {
        _worldSpaceValueText.text = string.Format("{0:0} / {1:0}", _currentValue, _maxValue);
    }
}
