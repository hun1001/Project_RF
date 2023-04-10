using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : CustomObject, IPoolReset
{
    [SerializeField]
    private ShellSO _shellSO = null;
    public ShellSO ShellSO => _shellSO;

    private CustomObject _owner = null;
    public CustomObject Owner => _owner;

    [SerializeField]
    private SoundBoxSO _shellSound = null;
    public SoundBoxSO ShellSound => _shellSound;

    public float Speed => _shellSO.Speed;

    private float _damage = 0;
    public float Damage => _damage;

    public float Penetration => _shellSO.Penetration;

    public void SetShell(CustomObject owner, float turretDamage)
    {
        _owner = owner;
        _damage = turretDamage + _shellSO.Damage;
    }

    public void PoolObjectReset()
    {
        _owner = null;
        _damage = 0;
        GetComponent<TrailRenderer>().Clear();
    }
}
