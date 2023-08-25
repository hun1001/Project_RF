using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyBar : Bar
{
    private readonly static Vector3 _offset = Vector3.up * 3.5f + Vector3.back;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        transform.position = transform.parent.position + _offset;
    }

    protected override void UpdateValueText()
    {
        
    }
}
