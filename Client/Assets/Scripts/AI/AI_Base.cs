using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using Event;
using Pool;

public abstract class AI_Base : MonoBehaviour
{
    [SerializeField]
    private Tank _tank = null;
    public Tank Tank => _tank;

    protected NavMeshPath _navMeshPath = null;
    private BehaviorTree _behaviorTree = null;

    private Tank_Move _tankMove = null;
    public Tank_Move TankMove => _tankMove;

    private Tank_Rotate _tankRotate = null;
    public Tank_Rotate TankRotate => _tankRotate;

    private Tank_Damage _tankDamage = null;
    public Tank_Damage TankDamage => _tankDamage;

    private Turret_Rotate _turretRotate = null;
    public Turret_Rotate TurretRotate => _turretRotate;

    private Turret_Attack _turretAttack = null;
    public Turret_Attack TurretAttack => _turretAttack;

    private Turret_AimLine _turretAimLine = null;
    public Turret_AimLine TurretAimLine => _turretAimLine;

    private Tank _target = null;
    public Tank Target => _target;

    private bool _isUpdate = false;

    protected virtual void Init()
    {
        _navMeshPath = new NavMeshPath();

        _tank = TankSpawn();

        _tankMove = _tank.GetComponent<Tank_Move>(ComponentType.Move);
        _tankRotate = _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate);
        _tankDamage = _tank.GetComponent<Tank_Damage>(ComponentType.Damage);

        _turretRotate = _tank.Turret.GetComponent<Turret_Rotate>(ComponentType.Rotate);
        _turretAttack = _tank.Turret.GetComponent<BossTurret_Attack>(ComponentType.Attack);
        _turretAimLine = _tank.Turret.GetComponent<Turret_AimLine>(ComponentType.AimLine);

        _target = FindObjectOfType<Player>().Tank;

        _tankDamage.ResetAction();
        _tankDamage.AddOnDeathAction(Pool);

        OnStart();

        _behaviorTree = SetBehaviorTree();
        _isUpdate = true;
    }

    private void Pool()
    {
        _isUpdate = false;
        PoolManager.Pool("AI", gameObject);
    }

    private void Update()
    {
        if(_isUpdate&&!_tank.IsDead)
        { 
            OnUpdate();
            _behaviorTree.Tick();
        }
    }

    protected abstract Tank TankSpawn();
    protected abstract BehaviorTree SetBehaviorTree();

    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
}
