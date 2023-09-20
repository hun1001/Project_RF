using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Event;
using Pool;

public class BMP_BossAI : BossAI_Base
{
    private Vector3 _moveTargetPosition = Vector3.zero;

    private bool _isUsedSkill = false;

    private Queue<Vector3> _pathQueue = new Queue<Vector3>();
    private Vector3 _currentTargetPosition = Vector3.zero;

    protected override BehaviorTree SetBehaviorTree()
    {
        RootNode rootNode = null;

        SelectorNode selectorNode = null;

        ConditionalNode setMoveTargetPositionExcutionNode = null;
        ExecutionNode moveExcutionNode = null;

        SequenceNode tankAttackSequenceNode = null;
        ConditionalNode checkTargetInAim = null;
        ExecutionNode atk2Target = null;

        SequenceNode tankDefenseSequenceNode = null;
        ConditionalNode checkTankHP = null;
        ExecutionNode shield = null;

        moveExcutionNode = new ExecutionNode(Move);
        setMoveTargetPositionExcutionNode = new ConditionalNode(SetMoveTargetPosition, moveExcutionNode);

        atk2Target = new ExecutionNode(() =>
        {
            Attack();
        });

        shield = new ExecutionNode(() =>
        {
            _isUsedSkill = true;
            TankDamage.SetHP(TankDamage.CurrentHealth + 50f);
            Tank.TankData.Armour += 10f;
        });

        checkTargetInAim = new ConditionalNode(() =>
        {
            TurretRotate.Rotate((Target.transform.position - Tank.transform.position).normalized);

            return TurretAimLine.IsAim;
        }, atk2Target);

        checkTankHP = new ConditionalNode(() =>
        {
            return TankDamage.CurrentHealth < Tank.TankData.HP * 0.30f && _isUsedSkill == false;
        }, shield);

        tankAttackSequenceNode = new SequenceNode(checkTargetInAim);
        tankDefenseSequenceNode = new SequenceNode(checkTankHP);

        selectorNode = new SelectorNode(tankAttackSequenceNode, tankDefenseSequenceNode, setMoveTargetPositionExcutionNode);

        rootNode = new RootNode(selectorNode);

        var behaviorTree = new BehaviorTree(rootNode);

        return behaviorTree;
    }

    protected override Tank TankSpawn()
    {
        return SpawnManager.Instance.SpawnUnit("BMP-130-2", transform.position, transform.rotation, GroupType.Enemy);
    }

    private void Start()
    {
        TankDamage.AddOnDeathAction(() =>
        {
            Destroy(this.gameObject);
            EventManager.TriggerEvent(EventKeyword.BossClear);
        });

        TankMove.AddOnCrashAction((_) =>
        {
            _moveTargetPosition = Vector3.zero;
        });
    }

    private void Attack()
    {
        int attackType = Random.Range(0, 10);
        if (attackType < 4)
        {
            (TurretAttack as BMPBossTurret_Attack).FireMissile(Target.transform.position);
        }
        else if (attackType < 8)
        {
            TurretAttack.Fire();
        }
        else
        {
            (TurretAttack as BMPBossTurret_Attack).FireMissile(Target.transform.position);
            TurretAttack.Fire();
        }
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
        int tryCount = 0;

        do
        {
            randomNextPosition = Target.transform.position + Random.insideUnitSphere * moveTargetPositionDistance;
            isCanMove = NavMesh.CalculatePath(Tank.transform.position, randomNextPosition, NavMesh.AllAreas, _navMeshPath);
        } while (!isCanMove && ++tryCount <= 100);

        if (!isCanMove)
        {
            Debug.Log("Can't move tryCount: " + tryCount);
            return false;
        }

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

    //private void Move(Vector3 position)
    //{
    //    if (NavMesh.CalculatePath(Tank.transform.position, position, NavMesh.AllAreas, _navMeshPath))
    //    {
    //        StartCoroutine(MoveTarget(0, _navMeshPath.corners.Length));

    //        for (int i = 0; i < _navMeshPath.corners.Length - 1; i++)
    //        {
    //            Debug.DrawLine(_navMeshPath.corners[i], _navMeshPath.corners[i + 1], Color.red, 5f);
    //        }
    //    }
    //    else
    //    {
    //        _moveTargetPosition = Vector3.zero;
    //    }
    //}

    //private IEnumerator MoveTarget(int index, int pathLength)
    //{
    //    if (index < pathLength)
    //    {
    //        float dis = Vector3.Distance(Tank.transform.position, _navMeshPath.corners[index]);
    //        while (dis > 1f)
    //        {
    //            if (dis < 20f)
    //            {
    //                TankMove.Move(0.6f);
    //            }
    //            else if (dis < 10f)
    //            {
    //                TankMove.Move(0.4f);
    //            }
    //            else
    //            {
    //                TankMove.Move(0.9f);
    //            }

    //            TankRotate.Rotate((_navMeshPath.corners[index] - Tank.transform.position).normalized);
    //            dis = Vector3.Distance(Tank.transform.position, _navMeshPath.corners[index]);
    //            yield return null;
    //        }

    //        StartCoroutine(MoveTarget(index + 1, pathLength));
    //    }
    //}
}
