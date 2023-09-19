using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KVBossTurret_Rotate : Turret_Rotate
{
    private Transform[] turretTransforms = null;

    private void Awake()
    {
        turretTransforms = new Transform[3] { Turret.TurretTransform, ((KVBossTurret)Turret).SecondTurret, ((KVBossTurret)Turret).ThirdTurret };
    }

    public void Rotate(Vector2 direction, int index)
    {
        if (direction != Vector2.zero)
        {
            Vector3 dir = new Vector3(-direction.x, 0, direction.y);
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            float maxRotationDelta = Turret.TurretData.RotationSpeed * Time.deltaTime;
            turretTransforms[index].rotation = Quaternion.RotateTowards(turretTransforms[index].rotation, Quaternion.Euler(0, 0, targetRotation.eulerAngles.y), maxRotationDelta);
        }
    }
}
