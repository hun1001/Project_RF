using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bullet/ShellSO")]
public class ShellSO : ScriptableObject
{
    public float Damage;
    public float Speed;
    public float Penetration;

    [Range(0, 90)]
    public float RicochetAngle;
}
