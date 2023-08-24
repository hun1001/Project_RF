using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Tank_Move : Tank_Component
{
    private float _maxSpeed = 0f;
    public float _currentSpeed = 0f;
    public float CurrentSpeed => _currentSpeed;
    private float _acceleration = 0f;
    private float _targetSpeed = 0f;

    private bool _collision = false;
    private bool _stop = false;

    private Tank_Sound _tankSound = null;
    private float _loadSoundDelay;
    private bool _isDepart = false;

    private Action<float> _onCrash = null;
    public void AddOnCrashAction(Action<float> action) => _onCrash += action;

    private void Awake()
    {
        TryGetComponent(out _tankSound);
    }

    private void Start()
    {
        _maxSpeed = (Instance as Tank).TankData.MaxSpeed;
        _acceleration = (Instance as Tank).TankData.Acceleration;
        _tankSound.StartEngineSound();
    }

    public void Move(float magnitude)
    {
        magnitude = Mathf.Clamp(magnitude, -1f, 1f);

        if (magnitude > 0)
        {
            if (_isDepart == false)
            {
                if (_loadSoundDelay <= 0f)
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
            _targetSpeed = magnitude * _maxSpeed;
            SpeedDeceleration();
            //_isDepart = false;
            _tankSound?.MoveSoundUpdate(0f);
        }

        _currentSpeed = _collision || _stop ? 0f : _currentSpeed;
        transform.Translate(Vector3.up * Time.deltaTime * _currentSpeed);

        if (_loadSoundDelay > 0f)
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
    }

    public void Stop()
    {
        _targetSpeed = 0f;
        if(_currentSpeed > 0)
        {
            _currentSpeed -= _acceleration * Time.deltaTime * 2;
        }else if(_currentSpeed < 0)
        {
            _currentSpeed += _acceleration * Time.deltaTime * 2;
        }
        _isDepart = false;
        _tankSound?.MoveSoundUpdate(0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") && CheckCrashBack(collision.contacts[0].point) == true)
        {
            _onCrash?.Invoke(_currentSpeed);
            _tankSound.PlaySound(SoundType.TankImpact, AudioMixerType.Sfx, 0.7f);

            _currentSpeed = 0;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Tank"))
        {
            _onCrash?.Invoke(_currentSpeed);

            Tank otherTank = collision.gameObject.GetComponent<Tank>();

            if (otherTank.TankSO.HP >= (Instance as Tank).TankSO.HP)
            {
                _currentSpeed = Mathf.Clamp(_currentSpeed * 0.5f, 0f, _maxSpeed);
                _tankSound.PlaySound(SoundType.TankImpact, AudioMixerType.Sfx, 0.7f);
                StartCoroutine(CrashRebound(collision.contacts[0].normal * 2.5f));
            }
            else
            {
                _currentSpeed = Mathf.Clamp(_currentSpeed * 0.5f, 0f, _maxSpeed);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall") && CheckCrashBack(other.contacts[0].point))
        {
            _collision = true;
            _currentSpeed = 0;
        }
        else
        {
            _collision = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _collision = false;
    }

    private bool CheckCrashBack(Vector2 collisionDir)
    {
        Vector2 dir = collisionDir - (Vector2)transform.position;

        float dot = Vector2.Dot(dir.normalized, transform.up);

        return dot >= 0f;
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

    public void TankRebound(Vector3 dir)
    {
        StartCoroutine(CrashRebound(dir));
    }

    public void SetEnableMove(bool enable)
    {
        _stop = !enable;
    }
}
