using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Active_Item : Item.Item_Base
{
    public abstract void ItemEquip(int idx);

    protected virtual void ClickEvent()
    {

    }

    protected virtual void DragEvent(Vector2 pos)
    {

    }
}
