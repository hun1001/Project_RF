using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Damage : Tank_Component
{
    private float _currentHealth = 0f;
    private float _maxHealth = 0f;

    private void Awake()
    {
        _maxHealth = Tank.TankStatSO.HP;
        _currentHealth = _maxHealth;
    }

    public void Damaged(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Debug.Log("Tank is dead");
        }
    }
}
