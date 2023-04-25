using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tank_Skill : Tank_Component
{
    [HideInInspector]
    public Image SkillImage = null;

    private float _coolTime = 0f;
    protected float CoolTime
    {
        get => _coolTime;
        set => _coolTime = value;
    }

    public abstract void UseSkill();

    private void Update()
    {
        if (_coolTime > 0)
        {
            _coolTime -= Time.deltaTime;
            SkillImage.fillAmount = 1 - (_coolTime / 10f);
        }
    }
}
