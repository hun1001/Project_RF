using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;

public class MissileTargetDisplay : MonoBehaviour
{
    public void SetTarget(Vector2 targetPosition, float duration = 1f)
    {
        Vector3 tp = targetPosition;
        tp.z = -0.1f;
        transform.position = tp;
        
        StartCoroutine(DisplayDisableCoroutine(duration));
    }
    
    private IEnumerator DisplayDisableCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        PoolManager.Pool("MissileTargetDisplay", this.gameObject);
    }
}
