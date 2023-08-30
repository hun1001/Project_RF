using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSubArmament : MonoBehaviour
{
    private Tank _tank = null;
    public Tank Tank => _tank;

    private Transform _firePoint = null;
    public Transform FirePoint => _firePoint;

    public BaseSubArmament Setting(Tank tank, Transform point)
    {
        _tank = tank;
        _firePoint = point;

        return this;
    }

    public abstract SubArmamentKeyActionType ActionType { get; }

    public virtual void Aim() { }
    public abstract void Fire();
}
