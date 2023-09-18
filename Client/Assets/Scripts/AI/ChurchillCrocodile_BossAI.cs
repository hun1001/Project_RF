using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchillCrocodile_BossAI : BossAI_Base
{
    protected override BehaviorTree SetBehaviorTree()
    {
        BehaviorTree behaviorTree = null;

        RootNode rootNode = null;

        return base.SetBehaviorTree();
    }

    protected override Tank TankSpawn()
    {
        return SpawnManager.Instance.SpawnUnit("ChurchillCrocodile", transform.position, transform.rotation, GroupType.Enemy);
    }
}
