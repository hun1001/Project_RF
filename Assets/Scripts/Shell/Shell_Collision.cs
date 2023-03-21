using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Shell_Collision : Shell_Component
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.SendMessage("Damaged", (Instance as Shell).ShellSO.Damage, SendMessageOptions.DontRequireReceiver);

        PoolManager.Pool(Instance.ID, gameObject);
    }
}
