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
        _tank = PoolManager.Get<Tank>("T-44", transform.position, transform.rotation).SetTank(GroupType.Enemy);

        RootNode rootNode = new RootNode();
        SequenceNode sequenceNode = new SequenceNode();

        rootNode.AddChild(sequenceNode);

        // 타겟이 살아있나 채크하는 노드
        ConditionalNode targetIsAliveNode = new ConditionalNode(() => _target != null && _target.gameObject.activeSelf == true);

        // 타겟이 살아있으면 타겟을 찾는 노드
        ExecutionNode findTargetNode = new ExecutionNode(() =>
        {
            _target = FindObjectOfType<Player>().Tank.transform;
        });

        // 타겟이 내 사정거리 안에 있는지 체크하는 노드
        ConditionalNode checkTargetInSightNode = new ConditionalNode(() =>
        {
            float tankDistance = _tank.Turret.CurrentShell.Speed * 2f;

            return Vector3.Distance(_tank.transform.position, _target.position) <= tankDistance;
        });

        // 타겟을 향해 이동하는 노드
        ExecutionNode move2TargetNode = new ExecutionNode(() =>
        {

        });

        // 타겟을 향해 조준하는 노드
        ExecutionNode aim2TargetNode = new ExecutionNode(() =>
        {

        });

        // 타겟을 향해 발사하는 노드
        ExecutionNode fireNode = new ExecutionNode(() =>
        {

        });

        _behaviorTree = new BehaviorTree(rootNode);


    }
}
