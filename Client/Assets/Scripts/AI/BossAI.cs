using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Event;

public class BossAI : MonoBehaviour
{
    private Tank _tank = null;
    private NavMeshPath _navMeshPath = null;
    private BehaviorTree _behaviorTree = null;

    private Tank_Move _tankMove = null;
    private Tank_Rotate _tankRotate = null;
    private Tank_Damage _tankDamage = null;

    private Turret_Rotate _turretRotate = null;
    private BossTurret_Attack _turretAttack = null;
    private Turret_AimLine _turretAimLine = null;

    private Tank _target = null;

    private Vector3 _moveTargetPosition = Vector3.zero;

    private bool _isUsedSkill = false;

    private void Awake()
    {
        _tank = SpawnManager.Instance.SpawnUnit("BMP-130-2", transform.position, transform.rotation, GroupType.Enemy);
        _navMeshPath = new NavMeshPath();

        _tankMove = _tank.GetComponent<Tank_Move>(ComponentType.Move);
        _tankRotate = _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate);
        _tankDamage = _tank.GetComponent<Tank_Damage>(ComponentType.Damage);

        _turretRotate = _tank.Turret.GetComponent<Turret_Rotate>(ComponentType.Rotate);
        _turretAttack = _tank.Turret.GetComponent<BossTurret_Attack>(ComponentType.Attack);
        _turretAimLine = _tank.Turret.GetComponent<Turret_AimLine>(ComponentType.AimLine);

        _target = FindObjectOfType<Player>().Tank;
    }

    private void Start()
    {
        _tankDamage.AddOnDeathAction(() =>
        {
            Destroy(this.gameObject);
            EventManager.TriggerEvent(EventKeyword.BossClear);
        });

        Bar hpBar = FindObjectOfType<InformationCanvas>().BossHpBar;
        hpBar.Setting(_tank.TankData.HP);

        _tankDamage.AddOnDamageAction(hpBar.ChangeValue);

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
            _moveTargetPosition = _target.transform.position + (Random.insideUnitSphere * 20f);
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
            _tankDamage.SetHP(_tankDamage.CurrentHealth + 50f);
            hpBar.ChangeValue(500f);
            _tank.TankData.Armour += 10f;
        });

        checkAroundTarget = new ConditionalNode(() =>
        {
            _target ??= FindObjectOfType<Player>().Tank;

            if (_moveTargetPosition == Vector3.zero || Vector3.Distance(_tank.transform.position, _moveTargetPosition) < 15f)
            {
                StopAllCoroutines();
                _navMeshPath.ClearCorners();
                return true;
            }

            return false;
        }, move2Target);


        checkTargetInAim = new ConditionalNode(() =>
        {
            _turretRotate.Rotate((_target.transform.position - _tank.transform.position).normalized);

            return _turretAimLine.IsAim;
        }, atk2Target);

        checkTankHP = new ConditionalNode(() =>
        {
            return _tankDamage.CurrentHealth < _tank.TankData.HP * 0.30f && _isUsedSkill == false;
        }, shield);

        tankMoveSequenceNode = new SequenceNode(checkAroundTarget);
        tankAttackSequenceNode = new SequenceNode(checkTargetInAim);
        tankDefenseSequenceNode = new SequenceNode(checkTankHP);

        selectorNode = new SelectorNode(tankMoveSequenceNode, tankAttackSequenceNode, tankDefenseSequenceNode);

        rootNode = new RootNode(selectorNode);

        _behaviorTree = new BehaviorTree(rootNode);
    }

    private float _delayTime = 0f;

    private void Update()
    {
        if (_delayTime < 1f)
        {
            _delayTime += Time.deltaTime;
        }
        else
        {
            _behaviorTree.Tick();
        }
    }

    private void Attack()
    {
        int attackType = Random.Range(0, 10);
        if (attackType < 4)
        {
            _turretAttack.FireMissile(_target.transform.position);
        }
        else if (attackType < 8)
        {
            _turretAttack.Fire();
        }
        else
        {
            _turretAttack.FireMissile(_target.transform.position);
            _turretAttack.Fire();
        }
    }

    private void Move(Vector3 position)
    {
        if (NavMesh.CalculatePath(_tank.transform.position, position, NavMesh.AllAreas, _navMeshPath))
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
            float dis = Vector3.Distance(_tank.transform.position, _navMeshPath.corners[index]);
            while (dis > 1f)
            {
                if (dis < 20f)
                {
                    _tankMove.Move(0.6f);
                }
                else if (dis < 10f)
                {
                    _tankMove.Move(0.4f);
                }
                else
                {
                    _tankMove.Move(0.9f);
                }

                _tankRotate.Rotate((_navMeshPath.corners[index] - _tank.transform.position).normalized);
                dis = Vector3.Distance(_tank.transform.position, _navMeshPath.corners[index]);
                yield return null;
            }

            StartCoroutine(MoveTarget(index + 1, pathLength));
        }
    }
}
