using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
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

    private Action<float> _onCrash = null;
    public void AddOnCrashAction(Action<float> action) => _onCrash += action;

    private BoxCollider2D _boxCollider2D = null;
    private Rigidbody2D _rigid = null;

    private void Awake()
    {
        (Instance as Tank).TryGetComponent(out _tankSound);
        TryGetComponent(out _boxCollider2D);
        TryGetComponent(out _rigid);
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
            _currentSpeed -= _acceleration * Time.deltaTime * 2;
        }
        else if (_currentSpeed < 0f) _currentSpeed = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            _onCrash?.Invoke(_currentSpeed);

            _currentSpeed = 0;
            StartCoroutine(CrashRebound(collision.contacts[0].normal * 2.5f));
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Tank"))
        {
            _onCrash?.Invoke(_currentSpeed);

            Tank otherTank = collision.gameObject.GetComponent<Tank>();

            if(otherTank.TankSO.HP >= (Instance as Tank).TankSO.HP)
            {
                _currentSpeed = 0;
                StartCoroutine(CrashRebound(collision.contacts[0].normal * 2.5f));
            }
            else
            {
                _currentSpeed = Mathf.Clamp(_currentSpeed - _maxSpeed * 0.5f, 0f, _maxSpeed);
            }
        }
    }

    private IEnumerator CrashRebound(Vector3 dir)
    {
        Vector3 targetPos = transform.position + dir;

        float elapsedTime = 0f;
        while (elapsedTime < 0.3f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, elapsedTime / 0.3f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
