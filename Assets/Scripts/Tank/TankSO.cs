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
    private CountryType _countryType = 0;
    public CountryType CountryType => _countryType;

    [SerializeField]
    private TankType _tankType = 0;
    public TankType TankType => _tankType;

    [SerializeField]
    private uint _tankTier = 0;
    public uint TankTier => _tankTier;

    [SerializeField]
    private uint _price = 0;
    public uint Price => _price;

    public void SetData(float hp, float armour, float maxSpeed, float acceleration, float rotationSpeed, CountryType countryType, TankType tankType, uint tier, uint price)
    {
        _hp = hp;
        _armour = armour;
        _maxSpeed = maxSpeed;

        _acceleration = acceleration;
        _rotationSpeed = rotationSpeed;

        _countryType = countryType;
        _tankType = tankType;
        _tankTier = tier;

        _price = price;
    }

    public TankSO Clone() => Instantiate(this);
}
