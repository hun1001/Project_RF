using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class TankAI : MonoBehaviour
{
    private Tank _tank = null;
    private Transform _target = null;

    private BehaviorTree _behaviorTree = null;

    private void Start()
    {
        _tank = PoolManager.Get<Tank>("T-44", transform.position, transform.rotation).SetGroupType(GroupType.Enemy);

        RootNode rootNode = new RootNode();
        SequenceNode sequenceNode = new SequenceNode();

        rootNode.AddChild(sequenceNode);

        _behaviorTree = new BehaviorTree(rootNode);


    }
}
