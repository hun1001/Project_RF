using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Move : Tank_Component
{
    private float _maxSpeed = 0f;
    private float _currentSpeed = 0f;
    private float _acceleration = 0f;
    private float _targetSpeed = 0f;

    private void Start()
    {
        _maxSpeed = (Instance as Tank).TankData.Speed;
        _acceleration = (Instance as Tank).TankData.Acceleration;
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

        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();

        if (Physics2D.Raycast(transform.position, transform.up, boxCollider2D.offset.y + boxCollider2D.size.y / 2, LayerMask.GetMask("Wall")))
        {
            _currentSpeed = 0;
        }

        transform.Translate(Vector3.up * Time.deltaTime * _currentSpeed);
    }
}
