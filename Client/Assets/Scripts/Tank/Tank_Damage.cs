using Event;
using Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Damage : Tank_Component
{
    private float _currentHealth = 0f;
    public float CurrentHealth => _currentHealth;
    private float _maxHealth = 0f;
    private float _amour = 0f;

    private Action<float> _onDamageAction = null;
    public void AddOnDamageAction(Action<float> action) => _onDamageAction += action;

    private Action _onDeathAction = null;
    public void AddOnDeathAction(Action action) => _onDeathAction += action;

    private void Start() => ResetData();

    public void ResetData()
    {
        _maxHealth = (Instance as Tank).TankData.HP;
        _currentHealth = _maxHealth;
        _amour = (Instance as Tank).TankData.Armour;
    }

    public void SetHP(float hp) => _currentHealth = hp > _maxHealth ? _maxHealth : hp < 0 ? 0 : hp;

    public void Damaged(float damage, float penetration, Vector3 hitPos)
    {
        //damage = Mathf.Round(penetration > (Instance as Tank).TankData.Armour ? 99999 : (penetration * 2) > (Instance as Tank).TankData.Armour ? (damage * 10) - ((Instance as Tank).TankData.Armour - (penetration * 3)) : (penetration * 3) > (Instance as Tank).TankData.Armour ? (damage * 10) - ((Instance as Tank).TankData.Armour - (penetration * 2)) : (penetration * 5) > (Instance as Tank).TankData.Armour ? (damage * 10) - ((Instance as Tank).TankData.Armour - (penetration / 2)) : 1);

        PopupText text = PoolManager.Get<PopupText>("PopupDamage", hitPos + Vector3.back * 5, Quaternion.identity);
        if (damage <= 0)
        {
            text.SetText("Armor!");
        }
        else
        {
            text.SetText(damage);
        }
        text.DoMoveText();

        damage *= -1;

        _currentHealth += damage;
        _onDamageAction?.Invoke(damage);

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

    public void Repair(float percent)
    {
        percent = percent * 0.01f;
        percent = _maxHealth * percent;
        if (_currentHealth + percent > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        else
        {
            _currentHealth += percent;
        }
        _onDamageAction?.Invoke(percent);
    }
}
