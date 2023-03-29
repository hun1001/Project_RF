using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using Event;

public class TankAI : MonoBehaviour
{
    private Tank _tank = null;
    private Transform _target = null;

    private BehaviorTree _behaviorTree = null;

    private void Start()
    {
        _tank = PoolManager.Get<Tank>("T-44", transform.position, transform.rotation).SetTank(GroupType.Enemy);

        EventManager.DeleteEvent(_tank.gameObject.GetInstanceID().ToString());
        EventManager.StartListening(_tank.gameObject.GetInstanceID().ToString(), () =>
        {
            GameWay_Base.Instance.RemainingEnemy--;
            GoodsManager.AddGoods(GoodsType.GameGoods, 2);
            EventManager.TriggerEvent("Recycling");
            if (GameWay_Base.Instance.RemainingEnemy <= 0)
            {
                GameWay_Base.Instance.StageClear();
            }
        });

        RootNode rootNode = null;
        WhileNode whileNode = null;
        SequenceNode sequenceNode = null;

        ConditionalNode checkAroundTarget = null;
        WhileNode targetInSight = null;
        ExecutionNode move2Target = null;

        ConditionalNode checkTargetInSight = null;
        WhileNode targetInAim = null;
        ExecutionNode aim2Target = null;

        ConditionalNode checkTargetInAim = null;
        ExecutionNode fire = null;

        move2Target = new ExecutionNode(() =>
        {
            Vector3 direction = (_target.position - _tank.transform.position).normalized;

            _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate).Rotate(direction);
            _tank.GetComponent<Tank_Move>(ComponentType.Move).Move(_tank.TankData.MaxSpeed);
        });

        aim2Target = new ExecutionNode(() =>
        {
            Vector3 direction = (_target.position - _tank.Turret.FirePoint.position).normalized;

            _tank.Turret.GetComponent<Turret_Rotate>(ComponentType.Rotate).Rotate(direction);
        });

        fire = new ExecutionNode(() =>
        {
            _tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).Fire();
        });

        targetInSight = new WhileNode(() =>
        {
            var c = Physics.OverlapSphere(_tank.transform.position, _tank.Turret.CurrentShell.Speed * 2f - 5f, LayerMask.GetMask("Tank"));
            foreach (var item in c)
            {
                if (item.GetComponent<Tank>().GroupType == GroupType.Player)
                {
                    _target = item.transform;
                    return true;
                }
            }
            return false;
        }, move2Target);

        targetInAim = new WhileNode(() =>
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
        }, aim2Target);

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
            var c = Physics.OverlapSphere(_tank.transform.position, _tank.Turret.CurrentShell.Speed * 2f - 5f, LayerMask.GetMask("Tank"));
            foreach (var item in c)
            {
                if (item.GetComponent<Tank>().GroupType == GroupType.Player)
                {
                    _target = item.transform;
                    return true;
                }
            }
            return false;
        }, targetInSight);

        checkTargetInSight = new ConditionalNode(() =>
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
        }, targetInAim);

        sequenceNode = new SequenceNode(checkAroundTarget, checkTargetInSight, checkTargetInAim);
        whileNode = new WhileNode(() =>
        {
            return true;
        }, sequenceNode);

        rootNode = new RootNode(whileNode);

        _behaviorTree = new BehaviorTree(rootNode);
        _behaviorTree.Execute();
    }
}
