using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBar : Bar
{
    private static Vector3 _offset = Vector3.down * 3.5f + Vector3.back;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        transform.position = transform.parent.position + _offset;
    }
}
