using Event;
using Pool;
using System;
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
    public void AddOnDeathAction(Action action)
    {
        _onDeathAction += action;
    }

    public void ResetAction()
    {
        _onDamageAction = null;
        _onDeathAction = null;
    }

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

        string str;

        sumDamage = Mathf.Clamp(sumDamage, 1, damage);

        sumDamage = (float)Math.Truncate(sumDamage);

        _lastHitDir = hitDir;

        float percent = sumDamage / _maxHealth;
        if (sumDamage == 1)
        {
            _tankSound.PlaySound(SoundType.TankHitVerySmall, AudioMixerType.Sfx);
            str = "<color=#ff4c4c><size=1.3>";
        }
        else if (percent >= 0.4)
        {
            _tankSound.PlaySound(SoundType.TankHitBig, AudioMixerType.Sfx);
            str = "<color=#a30000><size=2.5>";
        }
        else if (percent >= 0.2)
        {
            _tankSound.PlaySound(SoundType.TankHitMed, AudioMixerType.Sfx);
            str = "<color=#c21010><size=1.9>";
        }
        else
        {
            _tankSound.PlaySound(SoundType.TankHitSmall, AudioMixerType.Sfx);
            str = "<color=#e02828><size=1.5>";
        }

        PopupText text = PoolManager.Get<PopupText>("PopupDamage", hitPos + Vector3.back * 5, Quaternion.identity);
        text.SetText(str + sumDamage.ToString() + "</size></color>");
        text.DoMoveText();

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
        GameObject destroyTank = PoolManager.Get("Destroyed_Tank", transform.position, transform.rotation);
        //if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.TrainingScene)
        //{
        //    TrainingTankManager.DestroyedTankQueue.Enqueue(destroyTank);
        //}
        PoolManager.Get("BrickImpact", new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
        PoolManager.Get("explosion_stylized_large_originalFire_ShaderGraph", transform.position, Quaternion.Euler(-90, 0, 0));
        EventManager.TriggerEvent(gameObject.GetInstanceID().ToString());


        (Instance as Tank).GetComponent<Tank_Move>(ComponentType.Move)._currentSpeed = 0;

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
