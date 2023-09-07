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
        
    }

    private void SetMoveTargetPosition()
    {

    }

    private bool IsTargetAim()
    {
        return true;
    }

    private void Fire()
    {

    }
}
