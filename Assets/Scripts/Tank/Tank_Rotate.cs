using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Rotate : Tank_Component
{
    public void Rotate(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
