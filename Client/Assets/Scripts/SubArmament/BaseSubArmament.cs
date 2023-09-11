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

    private Action _onReloadAction = null;
    public void AddOnReloadAction(Action action) => _onReloadAction += action;

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
        _onReloadAction?.Invoke();
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(GetSATSO().ReloadTime);
        _curretBeltCapacity = GetSATSO().BeltCapacity;
    }
}
