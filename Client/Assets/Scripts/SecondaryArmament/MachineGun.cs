using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : SubArmament
{
    public override SubArmamentKeyActionType ActionType => SubArmamentKeyActionType.OnKeyHold;

    public override void Fire()
    {
        Debug.Log("MachineGun Fire");
    }
}
