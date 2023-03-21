using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Shell_Collision : Shell_Component
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // collision.SendMessage("Damaged", (Instance as Shell).Damage, (Instance as Shell).Penetration, SendMessageOptions.DontRequireReceiver);
        collision.GetComponent<Tank_Damage>().Damaged((Instance as Shell).Damage, (Instance as Shell).Penetration);
        PoolManager.Pool(Instance.ID, gameObject);
    }
}
