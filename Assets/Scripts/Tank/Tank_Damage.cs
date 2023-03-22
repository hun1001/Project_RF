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

    private void Awake()
    {
        _maxHealth = Tank.TankSO.HP;
        _currentHealth = _maxHealth;
        _amour = Tank.TankSO.Armour;
    }

    public void Damaged(float damage, float penetration)
    {
        // TODO : 아머 계산 생각하기
        damage *= -1;

        _onDamageAction?.Invoke(damage);
        _currentHealth += damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            EventManager.TriggerEvent(gameObject.GetInstanceID().ToString());
        }
    }
}
