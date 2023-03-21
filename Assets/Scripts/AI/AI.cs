using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class AI : CustomObject
{
    private Tank _tank = null;
    private Transform _target = null;

    private void Awake()
    {
        _tank = PoolManager.Get<Tank>("T-44", transform.position, transform.rotation);
    }

    private void Start()
    {
        _target = FindObjectOfType<Player>().Tank.transform;
    }

    private void Update()
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
