using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using Event;

public abstract class BossAI_Base : MonoBehaviour
{
    private Tank _tank = null;
    public Tank Tank => _tank;

    private NavMeshPath _navMeshPath = null;
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

    private void Awake()
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

        OnStart();

        _behaviorTree = SetBehaviorTree();
    }

    private void Update()
    {
        OnUpdate();
        _behaviorTree.Tick();
    }

    protected abstract Tank TankSpawn();
    protected abstract BehaviorTree SetBehaviorTree();

    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
}
