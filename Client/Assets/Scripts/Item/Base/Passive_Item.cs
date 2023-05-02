using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Item : Item.Item_Base
{
    public override void ItemEquip()
    {
    }

    protected virtual void AddEvent()
    {

    }

    protected virtual IEnumerator PersistentItem()
    {
        yield return null;
    }
}
