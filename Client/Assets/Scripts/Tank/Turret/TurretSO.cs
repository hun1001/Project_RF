using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Burst
{
    public float BurstReloadTime;
    public int MagazineSize;
}

[CreateAssetMenu(menuName = "SO/Tank/Turret/TurretSO")]
public class TurretSO : ScriptableObject
{
    public float RotationSpeed;
    public float ReloadTime;
    public float FOV;
    public float AtkPower;
    public float PenetrationPower;

    public bool IsBurst;
    public Burst BurstData;

    public List<Shell> Shells;

    public TurretSO Clone() => Instantiate(this);
}
