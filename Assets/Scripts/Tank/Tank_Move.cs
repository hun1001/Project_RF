using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Tank_Move : Tank_Component
{
    private float _maxSpeed = 0f;
    private float _currentSpeed = 0f;
    private float _acceleration = 0f;
    private float _targetSpeed = 0f;

    private Tank_Sound _tankSound = null;
    private bool _isDepart = false;

    private void Awake()
    {
        (Instance as Tank).TryGetComponent(out _tankSound);
    }

    private void Start()
    {
        _maxSpeed = (Instance as Tank).TankData.MaxSpeed;
        _acceleration = (Instance as Tank).TankData.Acceleration;
        _tankSound.StartEngineSound();
    }

    public void Move(float magnitude)
    {
        if (magnitude > 0)
        {
            if(_isDepart == false)
            {
                _isDepart = true;
                _tankSound.PlaySound(SoundType.Load, AudioMixerType.Sfx, 0.7f);
            }
            _targetSpeed = magnitude * _maxSpeed;

            if (_currentSpeed < _targetSpeed)
            {
                _currentSpeed += _acceleration * Time.deltaTime;
                _tankSound.MoveSoundUpdate(_currentSpeed /  _maxSpeed);
            }
            else
            {
                _currentSpeed = _targetSpeed;
                _tankSound.MoveSoundUpdate(1f);
            }
        }
        else
        {
            _targetSpeed = 0;
            _currentSpeed = 0;
            _isDepart = false;
            _tankSound.MoveSoundUpdate(0f);
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

        var rayData = Physics2D.RaycastAll(transform.position, transform.up, boxCollider2D.offset.y + boxCollider2D.size.y / 2);

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
