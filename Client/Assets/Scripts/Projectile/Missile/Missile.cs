using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private Vector3 _targetPosition = Vector3.zero;

    private void SetMissile(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    private IEnumerator MissileCoroutine()
    {
        yield return null;
        // 대충 실행끝나면 알아서 풀
    }
}
