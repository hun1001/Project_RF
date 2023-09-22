using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using DG.Tweening;

public class RepairPack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var tank = collision.GetComponent<Tank>();

        if (tank != null)
        {
            if(tank == FindObjectOfType<Player>().Tank)
            {
                tank.GetComponent<Tank_Damage>().Repair(20f);
                PoolManager.Pool("RepairPack", gameObject);
            }
        }
    }

    private void OnEnable()
    {
        transform.DORotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }
}
