using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using Event;

public class TankAI : MonoBehaviour
{
    private Tank _tank = null;
    private Transform _target = null;
    private Bar _hpBar = null;

    private bool _isAiming = false;

    private BehaviorTree _behaviorTree = null;

    private void Start()
    {
        _tank = SpawnManager.Instance.SpawnUnit("T-44", transform.position, transform.rotation, GroupType.Enemy);
        _hpBar = PoolManager.Get<Bar>("EnemyBar", _tank.transform.position, Quaternion.identity, _tank.transform);
        _hpBar.Setting(_tank.TankData.HP);

        EventManager.DeleteEvent(_tank.gameObject.GetInstanceID().ToString());
        EventManager.StartListening(_tank.gameObject.GetInstanceID().ToString(), () =>
        {
            GameWay_Base.Instance.RemainingEnemy--;
            //GoodsManager.AddGoods(GoodsType.GameGoods, 2);
            EventManager.TriggerEvent("Recycling");
            if (GameWay_Base.Instance.RemainingEnemy <= 0)
            {
                GameWay_Base.Instance.StageClear();
            }
        });

        _tank.GetComponent<Tank_Damage>(ComponentType.Damage).AddOnDamageAction(_hpBar.ChangeValue);

        RootNode rootNode = null;
        WhileNode whileNode = null;
        SequenceNode sequenceNode = null;

        ConditionalNode checkAroundTarget = null;
        ExecutionNode move2Target = null;

        ExecutionNode aim2Target = null;

        ConditionalNode checkTargetInAim = null;
        ExecutionNode fire = null;

        move2Target = new ExecutionNode(() =>
        {
            Vector3 direction = (_target.position - _tank.transform.position).normalized;

            float dis = Vector3.Distance(_tank.transform.position, _target.position);

            if (dis > 20f && !_isAiming)
            {
                _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate).Rotate(direction);
                _tank.GetComponent<Tank_Move>(ComponentType.Move).Move(0.9f);
            }
            else if (dis < 10)
            {
                _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate).Rotate(-direction);
                _tank.GetComponent<Tank_Move>(ComponentType.Move).Move(-0.4f);
            }
            else
            {
                _isAiming = true;
            }
        });

        aim2Target = new ExecutionNode(() =>
        {
            Vector3 direction = (_target.position - _tank.Turret.FirePoint.position).normalized;
            float dis = Vector3.Distance(_tank.Turret.FirePoint.position, _target.position);

            if (dis > _tank.Turret.CurrentShell.Speed * 2f)
            {
                _isAiming = false;
            }

            var r = Physics2D.Raycast(_tank.Turret.FirePoint.position, _tank.Turret.FirePoint.up, _tank.Turret.CurrentShell.Speed * 2f, LayerMask.GetMask("Tank"));
            if (r.collider == null || r.collider.GetComponent<Tank>().GroupType != GroupType.Player)
            {
                _tank.Turret.GetComponent<Turret_Rotate>(ComponentType.Rotate).Rotate(direction);
            }
        });

        fire = new ExecutionNode(() =>
        {
            _tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).Fire();
        });

        checkTargetInAim = new ConditionalNode(() =>
        {
            var r = Physics2D.Raycast(_tank.Turret.FirePoint.position, _tank.Turret.FirePoint.up, _tank.Turret.CurrentShell.Speed * 2f, LayerMask.GetMask("Tank"));
            if (r.collider != null)
            {
                if (r.collider.GetComponent<Tank>().GroupType == GroupType.Player)
                {
                    return true;
                }
            }
            return false;
        }, fire);

        checkAroundTarget = new ConditionalNode(() =>
        {
            var c = Physics2D.OverlapCircleAll(_tank.transform.position, _tank.Turret.CurrentShell.Speed * 2f, LayerMask.GetMask("Tank"));

            foreach (var item in c)
            {
                if (item.GetComponent<Tank>().GroupType == GroupType.Player)
                {
                    _target = item.transform;
                    return true;
                }
            }
            _isAiming = false;
            return false;
        }, move2Target);

        sequenceNode = new SequenceNode(checkAroundTarget, aim2Target, checkTargetInAim);

        whileNode = new WhileNode(() =>
        {
            return true;
        }, sequenceNode);

        rootNode = new RootNode(whileNode);

        _behaviorTree = new BehaviorTree(rootNode);
    }

    private void Update()
    {
        _behaviorTree.Tick();
    }
}
