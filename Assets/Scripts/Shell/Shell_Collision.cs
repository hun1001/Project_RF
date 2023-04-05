using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Shell_Collision : Shell_Component
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((Instance as Shell).Owner == collision.gameObject.GetComponent<CustomObject>())
        {
            return;
        }

        var pt = ((Instance as Shell).Owner as Tank);
        var et = collision.gameObject.GetComponent<Tank>();

        if (pt != null && et != null)
        {
            if (pt.GroupType == et.GroupType)
            {
                return;
            }
        }

        Vector2 normalVector = collision.contacts[0].normal;
        Vector2 incidentVector = -transform.up;

        int angle = (int)Vector2.Angle(incidentVector, normalVector);
        angle %= 90;

        if (angle >= 60)
        {
            Vector2 incidentDir = -incidentVector;
            Vector2 reflectionDir = Vector2.Reflect(incidentDir, normalVector);

            transform.up = reflectionDir;
        }
        else
        {
            collision.gameObject.GetComponent<Tank_Damage>().Damaged((Instance as Shell).Damage, (Instance as Shell).Penetration);
            PoolManager.Get("Explosion_APHE_01", transform.position, transform.rotation);
            PoolManager.Pool(Instance.ID, gameObject);
        }
    }
}
