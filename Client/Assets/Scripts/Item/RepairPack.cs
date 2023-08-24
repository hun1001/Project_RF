using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class RepairPack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var tank = collision.GetComponent<Tank>();

        if (tank != null)
        {
            Debug.Log("RepairPack");
            PoolManager.Pool("RepairPack", gameObject);

            tank.GetComponent<Tank_Damage>(ComponentType.Damage).Repair(20);
        }
    }
}
