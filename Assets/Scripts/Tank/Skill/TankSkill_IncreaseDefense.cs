using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSkill_IncreaseDefense : Tank_Skill
{
    public override void UseSkill()
    {
        if (CoolTime <= 0)
        {
            StartCoroutine(IncreaseDefense());
            CoolTime = 10f;
        }
    }

    private IEnumerator IncreaseDefense()
    {
        (Instance as Tank).TankData.Armour *= 2;
        yield return new WaitForSeconds(5f);
        (Instance as Tank).TankData.Armour /= 2;
    }
}
