using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Move : Bullet_Component
{
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * (Instance as Bullet).BulletSO.Speed);
    }
}
