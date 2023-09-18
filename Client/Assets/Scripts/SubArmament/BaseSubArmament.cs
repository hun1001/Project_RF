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
    private AudioClip _fireSound = null;

    private Tank _tank = null;
    public Tank Tank => _tank;

    private Transform _firePoint = null;
    public Transform FirePoint => _firePoint;

    private Action _onCoolingAction = null;
    public void AddOnCoolingAction(Action action) => _onCoolingAction += action;

    private int _curretBeltCapacity = 0;
    public int CurretBeltCapacity => _curretBeltCapacity;

    private bool _isCooling = false;

    private bool _isAiming = false;

    private bool _canReload = true;

    private AudioSourceController _audioSourceController = null;

    public BaseSubArmament Setting(Tank tank, Transform point)
    {
        _tank = tank;
        _firePoint = point;

        _curretBeltCapacity = GetSATSO().BeltCapacity;

        _audioSourceController = PoolManager.Get<AudioSourceController>("AudioSource", _firePoint);
        _audioSourceController.transform.localPosition = Vector3.zero;

        _audioSourceController.SetSound(_fireSound);
        _audioSourceController.SetGroup(AudioMixerType.Sfx);
        _audioSourceController.SetLoop();
        _audioSourceController.SetVolume(0.8f);

        return this;
    }

    public virtual void Aim()
    {
        if (TutorialManager.Instance.IsTutorial)
        {
            if (!TutorialManager.Instance.IsCanAttack)
            {
                return;
            }
        }

        _isAiming = true;
    }

    public virtual void Fire()
    {
        if (_curretBeltCapacity <= 0)
        {
            StartCoroutine(CoolingCoroutine());
            return;
        }

        if(_isCooling)
        {
            _isPlayingSound = false;
            _audioSourceController.Stop();
            return;
        }

        --_curretBeltCapacity;
        PlayFireSound();
        OnFire();
    }

    public virtual void StopFire()
    {
        if (TutorialManager.Instance.IsTutorial)
        {
            if (!TutorialManager.Instance.IsCanAttack)
            {
                return;
            }
        }

        _isPlayingSound = false;
        _audioSourceController.Stop();
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
        if (_isCooling)
        {
            yield break;
        }
        _onCoolingAction?.Invoke();
        _isCooling = true;
        yield return new WaitForSeconds(GetSATSO().ReloadTime);
        _curretBeltCapacity = GetSATSO().BeltCapacity;
        _isCooling = false;
    }

    private bool _isPlayingSound = false;

    private void PlayFireSound()
    {
        if (_isPlayingSound)
        {
            return;
        }

        _isPlayingSound = true;
        _audioSourceController.Play();
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
