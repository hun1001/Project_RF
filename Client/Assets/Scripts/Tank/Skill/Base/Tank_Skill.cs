using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tank_Skill : Tank_Component
{
    private float _coolTime = 0f;

    public abstract void UseSkill();

    private void Update()
    {
        if (_coolTime > 0)
        {
            _coolTime -= Time.deltaTime;
        }
    }
}
