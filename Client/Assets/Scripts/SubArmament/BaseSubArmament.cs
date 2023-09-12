using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pool;
using UnityEngine.Rendering;

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

    public BaseSubArmament Setting(Tank tank, Transform point)
    {
        _tank = tank;
        _firePoint = point;

        _curretBeltCapacity = GetSATSO().BeltCapacity;

        return this;
    }

    public abstract SubArmamentKeyActionType ActionType { get; }

    public virtual void Aim() { }
    public virtual void Fire()
    {
        if (_curretBeltCapacity <= 0)
        {
            Reload();
            return;
        }

        --_curretBeltCapacity;
        _onFireAction?.Invoke();
        PlayFireSound();
        OnFire();
    }

    protected abstract void OnFire();

    private void Reload()
    {
        if (_isReloading)
        {
            return;
        }
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
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
}
