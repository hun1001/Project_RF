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

        // 타겟이 살아있으면 타겟을 찾는 노드
        ExecutionNode findTargetNode = new ExecutionNode(() =>
        {
            _target = FindObjectOfType<Player>().Tank.transform;
        });

        // 타겟이 내 사정거리 안에 있는지 체크하는 노드(타겟이 내 사정거리 안에 있으면 false를 반환)
        ConditionalNode checkTargetInSightNode = new ConditionalNode(() =>
        {
            float tankDistance = _tank.Turret.CurrentShell.Speed * 2f;

            return !(Vector3.Distance(_tank.transform.position, _target.position) <= tankDistance);
        });

        // 타겟을 향해 이동하는 노드
        ExecutionNode move2TargetNode = new ExecutionNode(() =>
        {
            Vector3 direction = _target.position - _tank.transform.position;
            direction.z = 0;

            _tank.GetComponent<Tank_Move>(ComponentType.Move).Move(_tank.TankData.MaxSpeed);
            _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate).Rotate(direction);
        });

        // 타겟을 향해 조준하는 노드
        ExecutionNode aim2TargetNode = new ExecutionNode(() =>
        {
            Vector3 direction = _target.position - _tank.transform.position;
            direction.z = 0;

            _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate).Rotate(direction);
        });

        ConditionalNode checkTargetInAimNode = new ConditionalNode(() =>
        {
            var r = Physics2D.Raycast(_tank.transform.position, _tank.transform.up, _tank.Turret.CurrentShell.Speed * 2f, LayerMask.GetMask("Tank"));

            if (r.collider != null)
            {
                if (r.collider.gameObject == _target.gameObject)
                {
                    return true;
                }
            }
            return false;
        });

        // 타겟을 향해 발사하는 노드
        ExecutionNode fireNode = new ExecutionNode(() =>
        {
            _tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).Fire();
        });

        // 타겟이 살아있나 확인하는 노드
        WhileNode whileCheckAliveTargetNode = new WhileNode(() => _target != null && _target.gameObject.activeSelf == true);

        rootNode.AddChild(sequenceNode);
        sequenceNode.AddChild(findTargetNode);
        sequenceNode.AddChild(whileCheckAliveTargetNode);

        whileCheckAliveTargetNode.AddChild(checkTargetInSightNode);

        checkTargetInSightNode.AddChild(move2TargetNode);
        whileCheckAliveTargetNode.AddChild(aim2TargetNode);
        whileCheckAliveTargetNode.AddChild(checkTargetInAimNode);
        whileCheckAliveTargetNode.AddChild(fireNode);

        _behaviorTree = new BehaviorTree(rootNode);
        _behaviorTree.Execute();
    }
}
