using Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Damage : Tank_Component
{
    private float _currentHealth = 0f;
    private float _maxHealth = 0f;
    private float _amour = 0f;

    private Action<float> _onDamageAction = null;
    public void AddOnDamageAction(Action<float> action) => _onDamageAction += action;

    private Action _onDeathAction = null;
    public void AddOnDeathAction(Action action) => _onDeathAction += action;

    private void OnEnable()
    {
        _maxHealth = (Instance as Tank).TankData.HP;
        _currentHealth = _maxHealth;
        _amour = (Instance as Tank).TankData.Armour;
    }

    public void Damaged(float damage, float penetration)
    {
        // TODO : 아머 계산 생각하기
        damage *= -1;

        _onDamageAction?.Invoke(damage);
        _currentHealth += damage;

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        _currentHealth = 0;
        Pool.PoolManager.Pool(Instance.ID, gameObject);
        EventManager.TriggerEvent(gameObject.GetInstanceID().ToString());
        _onDeathAction?.Invoke();
    }
}
