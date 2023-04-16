using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Rotate : Turret_Component
{
    public void Rotate(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            Vector3 _direction = new Vector3(-direction.x, 0, direction.y);
            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            float maxRotationDelta = Turret.TurretData.RotationSpeed * Time.deltaTime;
            Turret.TurretTransform.rotation = Quaternion.RotateTowards(Turret.TurretTransform.rotation, Quaternion.Euler(0, 0, targetRotation.eulerAngles.y), maxRotationDelta);
        }
    }

    public void Default()
    {
        Turret.TurretTransform.rotation = Quaternion.RotateTowards(Turret.TurretTransform.rotation, Quaternion.Euler(0, 0, 0), Turret.TurretData.RotationSpeed * Time.deltaTime);
    }
}
