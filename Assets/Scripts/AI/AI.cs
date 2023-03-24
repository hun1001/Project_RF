using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using Event;

public class AI : CustomObject
{
    private Tank _tank = null;
    private Transform _target = null;

    private void Start()
    {
        _tank = PoolManager.Get<Tank>("T-44", transform.position, transform.rotation).SetGroupType(GroupType.Enemy);
        EventManager.DeleteEvent(_tank.gameObject.GetInstanceID().ToString());
        EventManager.StartListening(_tank.gameObject.GetInstanceID().ToString(), () =>
        {
            GameWay_Base.Instance.RemainingEnemy--;
            GoodsManager.AddGoods(GoodsType.GameGoods, 2);
            if (GameWay_Base.Instance.RemainingEnemy <= 0)
            {
                GameWay_Base.Instance.StageClear();
            }
        });
        _tank.Turret.CurrentShell = PoolManager.Get<Shell>("APHE", false);
        _target = FindObjectOfType<Player>().Tank.transform;
    }

    private void Update()
    {
        if (_target == null)
        {
            _target = FindObjectOfType<Player>().Tank.transform;
        }
        else
        {
            _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate).Rotate((_target.position - _tank.transform.position).normalized);
            _tank.Turret.GetComponent<Turret_Rotate>(ComponentType.Rotate).Rotate((_target.position - _tank.Turret.FirePoint.position).normalized);

            if (Vector3.Distance(_tank.transform.position, _target.position) > 15f)
            {
                _tank.GetComponent<Tank_Move>(ComponentType.Move).Move(1f);
            }
            else
            {
                _tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).Fire();
            }
        }
    }
}
