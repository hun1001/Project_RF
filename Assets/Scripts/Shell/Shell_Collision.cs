using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Shell_Collision : Shell_Component
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((Instance as Shell).Owner == collision.GetComponent<CustomObject>())
        {
            return;
        }

        var pt = ((Instance as Shell).Owner as Tank);
        var et = collision.GetComponent<Tank>();

        if (pt != null && et != null)
        {
            if (pt.GroupType == et.GroupType)
            {
                return;
            }
        }

        collision.GetComponent<Tank_Damage>().Damaged((Instance as Shell).Damage, (Instance as Shell).Penetration);
        PoolManager.Get("Explosion_APHE_01", transform.position, transform.rotation);
        PoolManager.Pool(Instance.ID, gameObject);
    }
}
