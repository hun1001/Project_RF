using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_AutoHeal : Passive_Item
{
    private float _currentHeal;

    private Tank_Damage _tankDamage;
    private WaitForSeconds _healDelay;

    public override void ItemEquip()
    {
        transform.parent.TryGetComponent(out _tankDamage);

        _healDelay = new WaitForSeconds(1f);
        StartCoroutine(PersistentItem());
    }

    protected override IEnumerator PersistentItem()
    {
        while (true)
        {
            _tankDamage.Repair(_currentHeal);

            yield return _healDelay;
        }
    }
}
