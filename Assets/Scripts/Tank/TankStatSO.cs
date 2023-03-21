using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Tank/TankStatSO")]
public class TankStatSO : ScriptableObject
{
    public float HP;
    public float Armour;
    public float Speed;
    public float Acceleration;
    public float RotationSpeed;
    public TankType TankType;
}
