using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

public class Item_Recycling : Passive_Item
{
    protected float _currentHeal;

    protected Tank_Damage _tankDamage;

    public override void ItemEquip()
    {
        AddEvent();
    }

    protected override void AddEvent()
    {
        EventManager.StartListening("Recycling", () =>
        {
            _tankDamage.Repair(_currentHeal);
        });
    }
}
