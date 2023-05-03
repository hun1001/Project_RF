using System;
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
    private float _loadSoundDelay;
    private bool _isDepart = false;

    private bool _isStop = false;
    private bool _isCrash = false;
    private Action<float> _onCrash = null;
    public void AddOnCrashAction(Action<float> action) => _onCrash += action;

    private BoxCollider2D _boxCollider2D = null;

    private void Awake()
    {
        (Instance as Tank).TryGetComponent(out _tankSound);
        TryGetComponent(out _boxCollider2D);
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
            if (_isDepart == false)
            {
                if(_loadSoundDelay <= 0f)
                {
                    _isDepart = true;
                    _loadSoundDelay = 3f;
                    _tankSound?.PlaySound(SoundType.Load, AudioMixerType.Sfx, 0.3f);
                }
            }
            _targetSpeed = magnitude * _maxSpeed;

            if (_currentSpeed < _targetSpeed)
            {
                _currentSpeed += _acceleration * Time.deltaTime;
            }
            else if (_currentSpeed > _targetSpeed)
            {
                SpeedDeceleration();
            }
            _tankSound?.MoveSoundUpdate(_currentSpeed / _maxSpeed);
        }
        else
        {
            _targetSpeed = 0;
            SpeedDeceleration();
            _isDepart = false;
            _tankSound?.MoveSoundUpdate(0f);
        }

        _isStop = false;
        var rayData = Physics2D.RaycastAll(transform.position, transform.up, _boxCollider2D.offset.y + _boxCollider2D.size.y * 0.5f);

        foreach (var ray in rayData)
        {
            if (ray.collider.gameObject.layer == LayerMask.NameToLayer("Wall") || (ray.collider.gameObject.layer == LayerMask.NameToLayer("Tank") && ray.collider.gameObject != gameObject))
            {
                if (_isCrash == false)
                {
                    _onCrash?.Invoke(_currentSpeed);
                    _isCrash = true;
                }
                _currentSpeed = 0;
                _isStop = true;
                break;
            }
        }
        if (_isCrash == true && _isStop == false)
        {
            _isCrash = false;
        }

        transform.Translate(Vector3.up * Time.deltaTime * _currentSpeed);
        if(_loadSoundDelay > 0f)
        {
            _loadSoundDelay -= Time.deltaTime;
        }
    }

    private void SpeedDeceleration()
    {
        if (_currentSpeed > _targetSpeed)
        {
            _currentSpeed -= _acceleration * Time.deltaTime;
        }
        else if (_currentSpeed < 0f) _currentSpeed = 0f;
    }
}
