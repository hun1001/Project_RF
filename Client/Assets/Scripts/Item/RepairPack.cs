using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class RepairPack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("RepairPack OnTriggerEnter2D");
        var tank = collision.GetComponent<Tank>();

        if (tank != null)
        {
            if(tank == FindObjectOfType<Player>().Tank)
            {
                tank.GetComponent<Tank_Damage>().Repair(20f);
                PoolManager.Pool("Assets/Prefabs/RepairPack.prefab", gameObject);
            }
        }
    }
}
