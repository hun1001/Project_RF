using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Tank/TankSO")]
public class TankSO : ScriptableObject
{
    public float HP;
    public float Armour;
    public float MaxSpeed;
    public float Acceleration;
    public float RotationSpeed;
    public TankType TankType;

    public TankSO Clone() => Instantiate(this);
}
