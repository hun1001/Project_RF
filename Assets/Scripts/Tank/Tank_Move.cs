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
        _maxSpeed = (Instance as Tank).TankData.MaxSpeed;
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

        Debug.Log(_currentSpeed);

        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();

        var rayData = Physics2D.RaycastAll(transform.position, transform.up, boxCollider2D.offset.y + boxCollider2D.size.y / 2);
        Debug.Log(_currentSpeed);

        foreach (var ray in rayData)
        {
            if (ray.collider.gameObject.layer == LayerMask.NameToLayer("Wall") || (ray.collider.gameObject.layer == LayerMask.NameToLayer("Tank") && ray.collider.gameObject != gameObject))
            {
                _currentSpeed = 0;
                break;
            }
        }


        transform.Translate(Vector3.up * Time.deltaTime * _currentSpeed);
    }
}
