using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Shell_Collision : Shell_Component
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Shell_Collision: " + collision.gameObject.name);
        PoolManager.Pool(gameObject.name, gameObject);
    }
}
