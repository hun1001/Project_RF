using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

public class Item_Recycling : Item.Item_Base
{
    protected float _currentHeal;

    protected Tank_Damage _tankDamage;

    protected override void CreateItem()
    {
        transform.parent.TryGetComponent(out _tankDamage);

        AddEvent();
    }

    protected virtual void AddEvent()
    {
        EventManager.StartListening("Recycling", () =>
        {
            _tankDamage.Repair(_currentHeal);
        });
    }
}
