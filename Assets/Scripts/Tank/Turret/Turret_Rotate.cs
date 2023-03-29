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
            //Turret.TurretTransform.rotation = Quaternion.Lerp(Turret.TurretTransform.rotation, Quaternion.Euler(0, 0, targetRotation.eulerAngles.y), Time.deltaTime / (1f / (Turret.TurretData.RotationSpeed / 360f)));
            Turret.TurretTransform.rotation = Quaternion.Slerp(Turret.TurretTransform.rotation, Quaternion.Euler(0, 0, targetRotation.eulerAngles.y), Turret.TurretData.RotationSpeed * Time.deltaTime);
        }
    }
}
