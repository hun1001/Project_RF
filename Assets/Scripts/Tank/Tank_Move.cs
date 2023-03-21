using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Move : Tank_Component
{
    private float _maxSpeed = 0f;
    private float _currentSpeed = 0f;
    private float _acceleration = 0f;
    private float _targetSpeed = 0f;

    private void Awake()
    {
        _maxSpeed = Tank.TankStatSO.Speed;
        _acceleration = Tank.TankStatSO.Acceleration;
    }

    public void Move(float magnitude)
    {
        if (magnitude > 0)
        {
            _targetSpeed = magnitude * _maxSpeed;

            if (_currentSpeed < _targetSpeed)
            {
                _currentSpeed += _acceleration * Time.deltaTime;
            }
            else
            {
                _currentSpeed = _targetSpeed;
            }
        }
        else
        {
            _targetSpeed = 0;
            _currentSpeed = 0;
            // if (_currentSpeed > _targetSpeed)
            // {
            //     _currentSpeed -= _acceleration * Time.deltaTime;
            // }
            // else
            // {
            //     _currentSpeed = _targetSpeed;
            // }
        }

        transform.Translate(Vector3.up * Time.deltaTime * _currentSpeed);
    }
}
