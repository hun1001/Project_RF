using Event;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankAINew : AI_Base
{
    string _id = string.Empty;

    public void Init(string id)
    {
        _id = id;
        base.Init();
        StopAllCoroutines();
    }

    protected override void OnStart()
    {
        TankDamage.AddOnDeathAction(() =>
        {
            EventManager.TriggerEvent(EventKeyword.EnemyDie);
            PoolManager.Get("RepairPack", Tank.transform.position + new Vector3(0, 0, -2f), Quaternion.identity);
            PoolManager.Get("TankDeathEffect", Tank.transform.position, Quaternion.Euler(0, 0, 0));
            TankMove.CurrentSpeed = 0;
        });

        EnemyBar enemyBar = Tank.GetComponentInChildren<EnemyBar>();

        if (enemyBar != null)
        {
            enemyBar.PoolObjectReset();
        }
        else
        {
            enemyBar = PoolManager.Get<EnemyBar>("EnemyBar", Tank.transform);
        }

        enemyBar.Setting(Tank.TankData.HP);

        TankDamage.AddOnDamageAction(enemyBar.ChangeValue);
        TankDamage.AddOnDamageAction((_) => enemyBar.Show());
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

        SequenceNode moveSequenceNode = null;
        ExecutionNode setMoveTargetPositionExcutionNode = null;
        ExecutionNode moveExcutionNode = null;

        ConditionalNode targetAimConditionNode = null;
        ExecutionNode fireExecutionNode = null;

        fireExecutionNode = new ExecutionNode(Fire);
        targetAimConditionNode = new ConditionalNode(IsTargetAim, fireExecutionNode);

        moveExcutionNode = new ExecutionNode(Move);
        setMoveTargetPositionExcutionNode = new ExecutionNode(SetMoveTargetPosition);

        moveSequenceNode = new SequenceNode(setMoveTargetPositionExcutionNode, moveExcutionNode);

        selectorNode = new SelectorNode(targetAimConditionNode, moveSequenceNode);

        behaviorTree = new BehaviorTree(rootNode);

        return behaviorTree;
    }

    private void Move()
    {
        Vector3 dir = Vector3.zero;
        float distance = 0f;

        for(int i = 0; i < _navMeshPath.corners.Length; i++)
        {
            dir = (_navMeshPath.corners[i] - Tank.transform.position).normalized;

            while (!(Tank.transform.rotation == Quaternion.LookRotation(dir)))
            {
                TankRotate.Rotate(dir);
            }

            distance = Vector3.Distance(Tank.transform.position, _navMeshPath.corners[i]);
            while(distance > 5f)
            {
                TankMove.Move(1f);
                distance = Vector3.Distance(Tank.transform.position, _navMeshPath.corners[i]);
            }

            while(TankMove.CurrentSpeed == 0)
            {
                TankMove.Stop();
            }
        }
    }

    private void SetMoveTargetPosition()
    {
        bool isCanMove = false;
        Vector3 randomNextPosition = Vector3.zero;
        NavMeshHit hit;

        do
        {
            randomNextPosition = Target.transform.position + Random.insideUnitSphere * 30f;
            isCanMove = NavMesh.SamplePosition(randomNextPosition, out hit, 30f, NavMesh.AllAreas);
        }while (isCanMove);

        NavMesh.CalculatePath(Tank.transform.position, hit.position, NavMesh.AllAreas, _navMeshPath);
    }

    private bool IsTargetAim()
    {
        return true;
    }

    private void Fire()
    {

    }
}
