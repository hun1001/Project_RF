using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_AutoHeal : Item.Item_Base
{
    [SerializeField]
    private float[] _upgradeHeal;
    private float _currentHeal;

    private Tank_Damage _tankDamage;
    private WaitForSeconds _healDelay;

    protected override void CreateItem()
    {
        _currentHeal = _upgradeHeal[0];
        transform.parent.TryGetComponent(out _tankDamage);

        _healDelay = new WaitForSeconds(1f);
        StartCoroutine(Heal());
    }

    protected override void UpgradeItem()
    {
        _currentHeal = _upgradeHeal[Item.ItemManager.Instance.HaveItemList[this]];
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
