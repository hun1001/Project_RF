using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bullet/BulletSO")]
public class BulletSO : ScriptableObject
{
    public float Damage;
    public float Speed;
    public float Penetration;

    [Range(0, 90)]
    public float RicochetAngle;
}
