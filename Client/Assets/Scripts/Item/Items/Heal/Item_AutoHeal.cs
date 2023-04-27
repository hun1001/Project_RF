using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_AutoHeal : Item.Item_Base
{
    private float _currentHeal;

    private Tank_Damage _tankDamage;
    private WaitForSeconds _healDelay;

    protected override void CreateItem()
    {
        transform.parent.TryGetComponent(out _tankDamage);

        _healDelay = new WaitForSeconds(1f);
        StartCoroutine(Heal());
    }
    private IEnumerator Heal()
    {
        while (true)
        {
            _tankDamage.Repair(_currentHeal);

            yield return _healDelay;
        }
    }
}
