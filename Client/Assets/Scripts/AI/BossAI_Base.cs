using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAI_Base : AI_Base
{
    protected override BehaviorTree SetBehaviorTree()
    {
        return null;
    }

    protected override Tank TankSpawn()
    {
        return null;
    }
}
