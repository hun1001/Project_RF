using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Tank/Turret/TurretSO")]
public class TurretSO : ScriptableObject
{
    public float Power;
    public float RotationSpeed;
    public float ReloadTime;
}
