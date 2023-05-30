using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Tank/TankSO")]
public class TankSO : ScriptableObject
{
    [SerializeField]
    private float _hp = 0;
    public float HP => _hp;

    [SerializeField]
    private float _armour = 0;
    public float Armour
    {
        get => _armour;
        set => _armour = value;
    }

    [SerializeField]
    private float _maxSpeed = 0;
    public float MaxSpeed => _maxSpeed;

    [SerializeField]
    private float _acceleration = 0;
    public float Acceleration => _acceleration;

    [SerializeField]
    private float _rotationSpeed = 0;
    public float RotationSpeed => _rotationSpeed;

    [SerializeField]
    private TankType _tankType = 0;
    public TankType TankType => _tankType;

    [SerializeField]
    private uint _tankTier = 0;
    public uint TankTier => _tankTier;

    [SerializeField]
    private bool _hasSkill = false;
    public bool HasSkill => _hasSkill;

    [Header("Item")]
    [Range(0, 2)]
    [SerializeField]
    private uint _activeItemInventorySize = 0;
    public uint ActiveItemInventorySize => _activeItemInventorySize;

    [Range(0, 3)]
    [SerializeField]
    private uint _passiveItemInventorySize = 0;
    public uint PassiveItemInventorySize => _passiveItemInventorySize;

    public void SetData(float hp, float armour, float maxSpeed, float acceleration, float rotationSpeed, TankType tankType, uint tier, bool hasSkill, uint activeItemInventorySize, uint passiveItemInventorySize)
    {
        _hp = hp;
        _armour = armour;
        _maxSpeed = maxSpeed;

        _acceleration = acceleration;
        _rotationSpeed = rotationSpeed;

        _tankType = tankType;
        _tankTier = tier;

        _hasSkill = hasSkill;

        _activeItemInventorySize = (uint)Mathf.Clamp(activeItemInventorySize, 0, 2);
        _passiveItemInventorySize = (uint)Mathf.Clamp(passiveItemInventorySize, 0, 3);
    }

    public TankSO Clone() => Instantiate(this);
}
