using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Rotate : Tank_Component
{
    private Vector3 _direction = Vector3.zero;

    public void Rotate(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            _direction.x = -direction.x;
            _direction.y = 0;
            _direction.z = direction.y;

            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetRotation.eulerAngles.y), Tank.TankStatSO.RotationSpeed * Time.deltaTime);
        }
    }
}
