using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Rotate : Tank_Component
{
    private Vector3 _direction = Vector3.zero;
    private float _rotationSpeed => (Instance as Tank).TankData.RotationSpeed;

    public void Rotate(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            direction.Normalize();

            _direction.x = -direction.x;
            _direction.y = 0;
            _direction.z = direction.y;

            Quaternion targetRotation = Quaternion.LookRotation(_direction);

            float maxRotationDelta = _rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetRotation.eulerAngles.y), maxRotationDelta);
        }
    }

    public void RotateRight()
    {
        transform.Rotate(0, 0, -_rotationSpeed * Time.deltaTime);
    }

    public void RotateLeft()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }
}
