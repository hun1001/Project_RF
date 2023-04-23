using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

public class Item_Recycling : Item.Item_Base
{
    [SerializeField]
    private float[] _upgradeHeal;
    protected float _currentHeal;

    protected Tank_Damage _tankDamage;

    protected override void CreateItem()
    {
        _currentHeal = _upgradeHeal[0];
        transform.parent.TryGetComponent(out _tankDamage);

        AddEvent();
    }

    protected override void UpgradeItem()
    {
        //_currentHeal = _upgradeHeal[Item.ItemManager.Instance.HaveItemList[this]];
    }

    protected virtual void AddEvent()
    {
        EventManager.StartListening("Recycling", () =>
        {
            _tankDamage.Repair(_currentHeal);
        });
    }
}
