using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseSubArmament : MonoBehaviour
{
    [SerializeField]
    protected SATSO _satSO = null;
    public abstract SATSO GetSATSO();

    [SerializeField]
    private Sprite _icon = null;
    public Sprite Icon => _icon;

    private Tank _tank = null;
    public Tank Tank => _tank;

    private Transform _firePoint = null;
    public Transform FirePoint => _firePoint;

    private Action _onFireAction = null;
    public void AddOnFireAction(Action action) => _onFireAction += action;

    private Action _onReloadStartAction = null;
    public void AddOnReloadAction(Action action) => _onReloadStartAction += action;

    private Action _onReloadEndAction = null;
    public void AddOnReloadEndAction(Action action) => _onReloadEndAction += action;

    private int _curretBeltCapacity = 0;
    public int CurretBeltCapacity => _curretBeltCapacity;

    public BaseSubArmament Setting(Tank tank, Transform point)
    {
        _tank = tank;
        _firePoint = point;

        _curretBeltCapacity = GetSATSO().BeltCapacity;

        return this;
    }

    public abstract SubArmamentKeyActionType ActionType { get; }

    public virtual void Aim() { }
    public void Fire()
    {
        if (_curretBeltCapacity <= 0)
        {
            Reload();
            return;
        }
        --_curretBeltCapacity;

        _onFireAction?.Invoke();
        OnFire();
    }

    protected abstract void OnFire();

    private void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        _onReloadStartAction?.Invoke();
        yield return new WaitForSeconds(GetSATSO().ReloadTime);
        _curretBeltCapacity = GetSATSO().BeltCapacity;
        _onReloadEndAction?.Invoke();
    }
}
