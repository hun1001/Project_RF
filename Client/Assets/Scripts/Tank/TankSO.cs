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
    public float Armour => _armour;

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

    public void SetData(float hp, float armour, float maxSpeed, float acceleration, float rotationSpeed, TankType tankType)
    {
        _hp = hp;
        _armour = armour;
        _maxSpeed = maxSpeed;
        _acceleration = acceleration;
        _rotationSpeed = rotationSpeed;
        _tankType = tankType;
    }

    public TankSO Clone() => Instantiate(this);
}
