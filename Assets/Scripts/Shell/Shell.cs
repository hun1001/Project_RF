using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : CustomObject
{
    [SerializeField]
    private ShellSO _shellSO = null;

    public float Speed => _shellSO.Speed;

    private float _damage = 0;
    public float Damage => _damage;

    public float Penetration => _shellSO.Penetration;

    public void SetShell(float turretDamage)
    {
        _damage = turretDamage + _shellSO.Damage;
    }
}
