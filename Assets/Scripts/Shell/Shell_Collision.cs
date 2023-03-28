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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 0.1f);
        Debug.DrawRay(hit.point, -transform.right, Color.blue, 5f);
        Debug.DrawRay(hit.point, collision.transform.up, Color.red, 5f);
        float d = Vector2.Dot(collision.transform.up, -transform.up);
        var dir = (Vector2)transform.right + (Vector2)collision.transform.up * d * 2f;
        Debug.DrawRay(hit.point, dir, Color.green, 5f);

        collision.GetComponent<Tank_Damage>().Damaged((Instance as Shell).Damage, (Instance as Shell).Penetration);
        PoolManager.Get("Explosion_APHE_01", transform.position, transform.rotation);
        PoolManager.Pool(Instance.ID, gameObject);
    }
}
