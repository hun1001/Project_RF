using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Passive_Item : Item.Item_Base
{
    public abstract void ItemEquip();

    protected virtual void AddEvent()
    {

    }

    protected virtual IEnumerator PersistentItem()
    {
        yield return null;
    }
}
