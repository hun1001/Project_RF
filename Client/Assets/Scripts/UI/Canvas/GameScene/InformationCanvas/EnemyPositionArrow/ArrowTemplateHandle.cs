using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTemplateHandle : MonoBehaviour
{
    private Tank _targetTank = null;
    
    public void SetTargetTank(Tank tank)
    {
        _targetTank = tank;
    }

    private void Update()
    {
        if (_targetTank.Equals(null))
        {
            return;
        }
        
        var targetPosition = _targetTank.transform.position;
        var targetPosition2D = new Vector2(targetPosition.x, targetPosition.z);
        var targetPosition2DNormalized = targetPosition2D.normalized;
        var targetPosition2DNormalizedAngle = Vector2.SignedAngle(Vector2.up, targetPosition2DNormalized);
        transform.localRotation = Quaternion.Euler(0, 0, targetPosition2DNormalizedAngle);
    }
}
