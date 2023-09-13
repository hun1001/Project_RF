using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pool;

public abstract class BaseSubArmament : MonoBehaviour
{
    [SerializeField]
    protected SATSO _satSO = null;
    public abstract SATSO GetSATSO();

    [SerializeField]
    private Sprite _icon = null;
    public Sprite Icon => _icon;

    [SerializeField]
    protected Shell _shell = null;

    [SerializeField]
    private AudioClip _fireAudioClip = null;

    private Tank _tank = null;
    public Tank Tank => _tank;

    private Transform _firePoint = null;
    public Transform FirePoint => _firePoint;

    private Action _onFireAction = null;
    public void AddOnFireAction(Action action) => _onFireAction += action;

    private Action _onReloadStartAction = null;
    public void AddOnReloadAction(Action action) => _onReloadStartAction += action;

    private int _curretBeltCapacity = 0;
    public int CurretBeltCapacity => _curretBeltCapacity;

    private bool _isReloading = false;

    private bool _isAiming = false;

    private bool _canReload = true;

    public BaseSubArmament Setting(Tank tank, Transform point)
    {
        _tank = tank;
        _firePoint = point;

        _curretBeltCapacity = GetSATSO().BeltCapacity;

        return this;
    }

    public virtual void Aim()
    {
        _isAiming = true;
    }

    public virtual void Fire()
    {
        if (_curretBeltCapacity <= 0)
        {
            StartCoroutine(CoolingCoroutine());
            return;
        }

        --_curretBeltCapacity;
        _onFireAction?.Invoke();
        PlayFireSound();
        OnFire();
    }

    public virtual void StopFire()
    {
        _isAiming = false;
    }

    protected abstract void OnFire();

    private void Reload()
    {
        if(!_canReload||_curretBeltCapacity == GetSATSO().BeltCapacity)
        {
            return;
        }
        StartCoroutine(ReloadRateCheckCoroutine());
        ++_curretBeltCapacity;
    }

    private IEnumerator ReloadRateCheckCoroutine()
    {
        _canReload = false;

        yield return new WaitForSeconds(1f / (GetSATSO().BeltCapacity / GetSATSO().ReloadTime));

        _canReload = true;
    }

    private IEnumerator CoolingCoroutine()
    {
        if (_isReloading)
        {
            yield break;
        }
        _onReloadStartAction?.Invoke();
        _isReloading = true;
        yield return new WaitForSeconds(GetSATSO().ReloadTime);
        _curretBeltCapacity = GetSATSO().BeltCapacity;
        _isReloading = false;
    }

    private void PlayFireSound()
    {
        var audioSource = PoolManager.Get<AudioSourceController>("AudioSource", _firePoint);
        audioSource.SetSound(_fireAudioClip);
        audioSource.SetGroup(AudioMixerType.Sfx);
        audioSource.SetVolume(1f);
        audioSource.Play();
    }

    private void Update()
    {
        if(_isAiming) 
        {
            Fire();
        }
        else
        {
            Reload();
        }
    }
}
