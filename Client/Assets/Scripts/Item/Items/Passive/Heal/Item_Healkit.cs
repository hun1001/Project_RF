using Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Healkit : Item_Recycling
{
    protected override void AddEvent()
    {
        EventManager.StartListening("Clear", () =>
        {
            _tankDamage.Repair(_currentHeal);
        });
    }
}
