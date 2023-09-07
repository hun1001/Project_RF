using Event;
using Pool;
using UnityEngine;
using UnityEngine.AI;

public class TankAINew : AI_Base
{
    string _id = string.Empty;
    private bool _isSettingDefinition;

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

        ConditionalNode setMoveTargetPositionExcutionNode = null;
        ExecutionNode moveExcutionNode = null;

        ConditionalNode targetAimConditionNode = null;
        ExecutionNode fireExecutionNode = null;

        fireExecutionNode = new ExecutionNode(Fire);
        targetAimConditionNode = new ConditionalNode(IsTargetAim, fireExecutionNode);

        moveExcutionNode = new ExecutionNode(Move);
        setMoveTargetPositionExcutionNode = new ConditionalNode(SetMoveTargetPosition, moveExcutionNode);

        selectorNode = new SelectorNode(targetAimConditionNode, setMoveTargetPositionExcutionNode);

        rootNode = new RootNode(selectorNode);

        behaviorTree = new BehaviorTree(rootNode);

        return behaviorTree;
    }

    private void Move()
    {
        Vector3 dir = Vector3.zero;
        float distance = 0f;

        Debug.Log("Move " + _navMeshPath.corners.Length);

        for(int i = 0; i < _navMeshPath.corners.Length - 1; i++)
        {
            Debug.DrawLine(_navMeshPath.corners[i], _navMeshPath.corners[i+1], Color.red, 5f);
        }
    }

    private bool SetMoveTargetPosition()
    {
        if(Vector3.Distance(Tank.transform.position, Target.transform.position) <= 30)
        {
            return false;
        }

        bool isCanMove = false;
        Vector3 randomNextPosition = Vector3.zero;

        do
        {
            randomNextPosition = Target.transform.position + Random.insideUnitSphere * 30f;
            isCanMove = NavMesh.CalculatePath(Tank.transform.position, randomNextPosition, NavMesh.AllAreas, _navMeshPath);
        } while (!isCanMove);

        return true;
    }

    private bool IsTargetAim()
    {
        float timer = 0f;
        Vector3 dir = (Target.transform.position - Tank.transform.position);

        while (!TurretAimLine.IsAim && timer < 5f)
        {
            TurretRotate.Rotate(dir.normalized);
            timer += Time.deltaTime;
        }

        return TurretAimLine.IsAim;
    }

    private void Fire()
    {
        TurretAttack.Fire();
    }
}
