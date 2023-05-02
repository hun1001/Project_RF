using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Active_Item : Item.Item_Base
{
    protected int _idx;

    public abstract void ItemEquip(int idx);

    protected virtual void ClickEvent()
    {

    }

    protected virtual void DragEvent()
    {

    }
}
