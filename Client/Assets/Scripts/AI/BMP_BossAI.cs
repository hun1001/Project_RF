using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Event;

public class BMP_BossAI : BossAI_Base
{
    private Vector3 _moveTargetPosition = Vector3.zero;

    private bool _isUsedSkill = false;

    protected override BehaviorTree SetBehaviorTree()
    {
        RootNode rootNode = null;

        SelectorNode selectorNode = null;

        SequenceNode tankMoveSequenceNode = null;
        ConditionalNode checkAroundTarget = null;
        ExecutionNode move2Target = null;

        SequenceNode tankAttackSequenceNode = null;
        ConditionalNode checkTargetInAim = null;
        ExecutionNode atk2Target = null;

        SequenceNode tankDefenseSequenceNode = null;
        ConditionalNode checkTankHP = null;
        ExecutionNode shield = null;

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

        shield = new ExecutionNode(() =>
        {
            _isUsedSkill = true;
            TankDamage.SetHP(TankDamage.CurrentHealth + 50f);
            Tank.TankData.Armour += 10f;
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

        checkTankHP = new ConditionalNode(() =>
        {
            return TankDamage.CurrentHealth < Tank.TankData.HP * 0.30f && _isUsedSkill == false;
        }, shield);

        tankMoveSequenceNode = new SequenceNode(checkAroundTarget);
        tankAttackSequenceNode = new SequenceNode(checkTargetInAim);
        tankDefenseSequenceNode = new SequenceNode(checkTankHP);

        selectorNode = new SelectorNode(tankMoveSequenceNode, tankAttackSequenceNode, tankDefenseSequenceNode);

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
}
