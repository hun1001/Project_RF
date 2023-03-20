using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Player : MonoBehaviour
{


    void Start()
    {
        GameObject t = PoolManager.Get("T-44");
    }

    void Update()
    {

    }
}
