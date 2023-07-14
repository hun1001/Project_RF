using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankAI : BossAI_Base
{
    private NavMeshPath _navMeshPath = null;
    private Vector3 _moveTargetPosition = Vector3.zero;

    string _id = string.Empty;

    public void Init(string id)
    {
        _id = id;
    }

    protected override void OnStart()
    {
        _navMeshPath = new NavMeshPath();
    }

    protected override Tank TankSpawn()
    {
        return SpawnManager.Instance.SpawnUnit(_id, transform.position, transform.rotation, GroupType.Enemy);
    }

    protected override BehaviorTree SetBehaviorTree()
    {
        BehaviorTree behaviorTree = null;

        RootNode rootNode = null;

        SelectorNode selectorNode = null;

        SequenceNode tankMoveSequenceNode = null;
        ConditionalNode checkAroundTarget = null;
        ExecutionNode move2Target = null;

        SequenceNode tankAttackSequenceNode = null;
        ConditionalNode checkTargetInAim = null;
        ExecutionNode atk2Target = null;

        move2Target = new ExecutionNode(() =>
        {
            _moveTargetPosition = Target.transform.position + (Random.insideUnitSphere * 20f);
            _moveTargetPosition.z = 0f;
            Move(_moveTargetPosition);
        });

        atk2Target = new ExecutionNode(() =>
        {
            Attack();
        });

        checkAroundTarget = new ConditionalNode(() =>
        {
            if (_moveTargetPosition == Vector3.zero || Vector3.Distance(Tank.transform.position, _moveTargetPosition) < 15f)
            {
                StopAllCoroutines();
                _navMeshPath.ClearCorners();
                return true;
            }

            return false;
        }, move2Target);


        checkTargetInAim = new ConditionalNode(() =>
        {
            TurretRotate.Rotate((Target.transform.position - Tank.transform.position).normalized);

            return TurretAimLine.IsAim;
        }, atk2Target);

        tankMoveSequenceNode = new SequenceNode(checkAroundTarget);
        tankAttackSequenceNode = new SequenceNode(checkTargetInAim);

        selectorNode = new SelectorNode(tankMoveSequenceNode, tankAttackSequenceNode);

        rootNode = new RootNode(selectorNode);

        return behaviorTree;
    }

    private void Move(Vector3 position)
    {
        if (NavMesh.CalculatePath(Tank.transform.position, position, NavMesh.AllAreas, _navMeshPath))
        {
            StartCoroutine(MoveTarget(0, _navMeshPath.corners.Length));

            for (int i = 0; i < _navMeshPath.corners.Length - 1; i++)
            {
                Debug.DrawLine(_navMeshPath.corners[i], _navMeshPath.corners[i + 1], Color.red, 5f);
            }
        }
        else
        {
            _moveTargetPosition = Vector3.zero;
        }
    }

    private IEnumerator MoveTarget(int index, int pathLength)
    {
        if (index < pathLength)
        {
            float dis = Vector3.Distance(Tank.transform.position, _navMeshPath.corners[index]);
            while (dis > 1f)
            {
                if (dis < 20f)
                {
                    TankMove.Move(0.6f);
                }
                else if (dis < 10f)
                {
                    TankMove.Move(0.4f);
                }
                else
                {
                    TankMove.Move(0.9f);
                }

                TankRotate.Rotate((_navMeshPath.corners[index] - Tank.transform.position).normalized);
                dis = Vector3.Distance(Tank.transform.position, _navMeshPath.corners[index]);
                yield return null;
            }

            StartCoroutine(MoveTarget(index + 1, pathLength));
        }
    }

    private void Attack()
    {

    }
}
