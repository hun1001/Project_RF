using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : CustomObject, IPoolReset
{
    [SerializeField]
    private ShellSO _shellSO = null;
    public ShellSO ShellSO => _shellSO;

    [SerializeField]
    private Sprite _shellSprite = null;
    public Sprite ShellSprite => _shellSprite;

    private CustomObject _owner = null;
    public CustomObject Owner => _owner;

    [SerializeField]
    private SoundBoxSO _shellSound = null;
    public SoundBoxSO ShellSound => _shellSound;

    [SerializeField]
    private string _shellExplosionEffectAddress = string.Empty;
    public string ShellExplosionEffectAddress => _shellExplosionEffectAddress;

    public float Speed => _shellSO.Speed;

    private float _damage = 0;
    public float Damage => _damage;

    private float _penetration = 0;
    public float Penetration => _penetration;

    public void SetShellPrefabs(string id, ShellSO shellSO, Sprite shellSprite)
    {
        ID = id;
        _shellSO = shellSO;
        _shellSprite = shellSprite;
    }

    public void SetShell(CustomObject owner, float atkPower = 0, float penetrationPower = 0)
    {
        _owner = owner;
        if (atkPower == 0 || penetrationPower == 0)
        {
            _damage = _shellSO.Damage;
            _penetration = _shellSO.Penetration;
        }
        else
        {
            _damage = Mathf.Round(Mathf.Pow((atkPower * 0.035f), 3.5f) * _shellSO.Damage) + penetrationPower;
            _penetration = Mathf.Round(atkPower * penetrationPower * _shellSO.Penetration / 3000f);
        }
    }

    public void PoolObjectReset()
    {
        _owner = null;
        _damage = 0;
        GetComponent<TrailRenderer>().Clear();
    }
}
