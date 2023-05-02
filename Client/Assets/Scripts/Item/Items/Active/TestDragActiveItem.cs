using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDragActiveItem : Active_Item
{
    public override void ItemEquip(int idx)
    {
        GearManager.Instance.ControllerCanvas.ButtonGroup.SetDragButton(idx, DragEvent, true);
    }

    protected override void DragEvent(Vector2 pos)
    {
        Debug.Log("Drag!");
    }
}
