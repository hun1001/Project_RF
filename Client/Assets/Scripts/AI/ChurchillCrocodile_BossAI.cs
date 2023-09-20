using Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChurchillCrocodile_BossAI : BossAI_Base
{
    private Queue<Vector3> _pathQueue = new Queue<Vector3>();
    private Vector3 _currentTargetPosition = Vector3.zero;

    private ChurchillCrocodileBossTurret_Attack _turretAttack => TurretAttack as ChurchillCrocodileBossTurret_Attack;

    private void Start()
    {
        TankDamage.AddOnDeathAction(() =>
        {
            Destroy(this.gameObject);
            EventManager.TriggerEvent(EventKeyword.EnemyDie);
        });
    }

    protected override BehaviorTree SetBehaviorTree()
    {
        BehaviorTree behaviorTree = null;

        RootNode rootNode = null;

        SelectorNode selectorNode = null;

        ConditionalNode setMoveTargetPositionExcutionNode = null;
        ExecutionNode moveExcutionNode = null;

        ConditionalNode targetAimConditionNode = null;
        ExecutionNode fireExecutionNode = null;

        moveExcutionNode = new ExecutionNode(Move);
        setMoveTargetPositionExcutionNode = new ConditionalNode(SetMoveTargetPosition, moveExcutionNode);

        fireExecutionNode = new ExecutionNode(Fire);
        targetAimConditionNode = new ConditionalNode(IsTargetAim, fireExecutionNode);

        selectorNode = new SelectorNode(targetAimConditionNode, setMoveTargetPositionExcutionNode);

        rootNode = new RootNode(selectorNode);

        behaviorTree = new BehaviorTree(rootNode);

        return behaviorTree;
    }

    protected override Tank TankSpawn()
    {
        return SpawnManager.Instance.SpawnUnit("ChurchillCrocodile", transform.position, transform.rotation, GroupType.Enemy);
    }

    private void Move()
    {
        if (Vector3.Distance(_currentTargetPosition, Tank.transform.position) < 2f)
        {
            if (_pathQueue.Count > 0)
            {
                _currentTargetPosition = _pathQueue.Dequeue();
            }
            else
            {
                _pathQueue.Clear();
                return;
            }
        }

        Vector3 dir = (_currentTargetPosition - Tank.transform.position);

        TankMove.Move(dir.magnitude / 10);
        TankRotate.Rotate(dir.normalized);
    }

    private bool SetMoveTargetPosition()
    {
        if (_pathQueue.Count > 0)
        {
            return true;
        }

        if (Vector3.Distance(Tank.transform.position, Target.transform.position) > 10 && (TurretAimLine.IsAim && !TurretAttack.IsReload))
        {
            return false;
        }

        bool isCanMove = false;
        Vector3 randomNextPosition = Vector3.zero;

        float moveTargetPositionDistance = TurretAttack.IsReload ? 30f : 100f;

        do
        {
            randomNextPosition = Target.transform.position + Random.insideUnitSphere * moveTargetPositionDistance;
            isCanMove = NavMesh.CalculatePath(Tank.transform.position, randomNextPosition, NavMesh.AllAreas, _navMeshPath);
        } while (!isCanMove);

        for (int i = 0; i < _navMeshPath.corners.Length - 1; ++i)
        {
            Debug.DrawLine(_navMeshPath.corners[i], _navMeshPath.corners[i + 1], Color.green, 10f);
        }

        for (int i = 0; i < _navMeshPath.corners.Length; ++i)
        {
            _pathQueue.Enqueue(_navMeshPath.corners[i]);
        }

        _currentTargetPosition = _pathQueue.Dequeue();

        return true;
    }

    private bool IsTargetAim()
    {
        Vector3 dir = (Target.transform.position - Tank.transform.position);
        TurretRotate.Rotate(dir.normalized);

        return TurretAimLine.IsAim && dir.magnitude <= 50f && !TurretAttack.IsReload;
    }

    private void Fire()
    {
        _turretAttack.Fire();
        _turretAttack.Flamethrow();
    }
}
