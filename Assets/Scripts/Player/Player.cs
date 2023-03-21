using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using Util;

public class Player : CustomObject
{
    [SerializeField]
    private Joystick _moveJoystick = null;

    [SerializeField]
    private Joystick _attackJoystick = null;

    [SerializeField]
    private CameraManager _cameraManager = null;

    private Tank _tank = null;
    public Tank Tank => _tank;

    void Awake()
    {
        _tank = PoolManager.Get<Tank>("T-44");

        _cameraManager.SetPlayer(_tank.transform);
        _attackJoystick.AddOnPointerUpAction(_tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).Fire);
    }

    void Update()
    {
        _tank.GetComponent<Tank_Move>(ComponentType.Move).Move(_moveJoystick.Magnitude);
        _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate).Rotate(_moveJoystick.Direction);
        _tank.Turret.GetComponent<Turret_Rotate>(ComponentType.Rotate).Rotate(_attackJoystick.Direction);
    }
}
