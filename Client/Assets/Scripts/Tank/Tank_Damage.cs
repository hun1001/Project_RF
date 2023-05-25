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

    private Vector2 _lastHitDir = Vector2.zero;
    public Vector2 LastHitDir => _lastHitDir;

    private Tank_Sound _tankSound = null;

    private Action<float> _onDamageAction = null;
    public void AddOnDamageAction(Action<float> action) => _onDamageAction += action;

    private Action _onDeathAction = null;
    public void AddOnDeathAction(Action action) => _onDeathAction += action;

    private void Awake()
    {
        TryGetComponent(out _tankSound);
    }

    private void Start() => ResetData();

    public void ResetData()
    {
        _maxHealth = (Instance as Tank).TankData.HP;
        _currentHealth = _maxHealth;
        _amour = (Instance as Tank).TankData.Armour;
    }

    public void SetHP(float hp) => _currentHealth = hp > _maxHealth ? _maxHealth : hp < 0 ? 0 : hp;

    public void Damaged(float damage, float penetration, Vector3 hitPos, Vector2 hitDir)
    {
        float decreaseDamage = (1 - _amour / penetration) * 2 * damage;

        float sumDamage = damage * UnityEngine.Random.Range(0.9f, 1.1f) + decreaseDamage;

        sumDamage = Mathf.Clamp(sumDamage, 1, damage);

        sumDamage = (float)Math.Truncate(sumDamage);

        PopupText text = PoolManager.Get<PopupText>("PopupDamage", hitPos + Vector3.back * 5, Quaternion.identity);
        text.SetText(sumDamage);
        text.DoMoveText();

        _lastHitDir = hitDir;

        float percent = sumDamage / _maxHealth;
        if (sumDamage == 1)
        {
            _tankSound.PlaySound(SoundType.TankHitVerySmall, AudioMixerType.Sfx);
        }
        else if (percent >= 0.4)
        {
            _tankSound.PlaySound(SoundType.TankHitBig, AudioMixerType.Sfx);
        }
        else if (percent >= 0.2)
        {
            _tankSound.PlaySound(SoundType.TankHitMed, AudioMixerType.Sfx);
        }
        else
        {
            _tankSound.PlaySound(SoundType.TankHitSmall, AudioMixerType.Sfx);
        }

        sumDamage *= -1;

        _currentHealth += sumDamage;
        _onDamageAction?.Invoke(sumDamage);

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        _currentHealth = 0;
        _tankSound.PlaySound(SoundType.TankDestroy, AudioMixerType.Sfx);
        PoolManager.Pool(Instance.ID, gameObject);
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
