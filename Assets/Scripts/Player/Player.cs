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

    [SerializeField]
    private Bar _hpBar = null;

    private Tank _tank = null;
    public Tank Tank => _tank;

    protected override void Awake()
    {
        base.Awake();

        Debug.Log("Player Awake");
        _tank = PoolManager.Get<Tank>("T-44");
        _tank.tag = "Player";

        _cameraManager.SetPlayer(_tank.transform);
        _attackJoystick.AddOnPointerUpAction(_tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).Fire);
        _hpBar.Setting(_tank.TankSO.HP);

        // TODO : 연동이 잘 안되는 경우 존재 해결 필요
        _tank.GetComponent<Tank_Damage>(ComponentType.Damage).AddOnDamageAction(_hpBar.ChangeValue);
        _tank.GetComponent<Tank_Damage>(ComponentType.Damage).AddOnDamageAction((a) => _cameraManager.CameraShake(2.5f, 2, 0.1f));
        _tank.GetComponent<Tank_Damage>(ComponentType.Damage).AddOnDeathAction(() =>
        {
            Debug.Log("Player Death");
            Time.timeScale = 0;

            StartCoroutine(Change());
        });

        _tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).AddOnFireAction(() => _cameraManager.CameraZoomInEffect(5f, 0.1f, 0.1f));
    }

    private IEnumerator Change()
    {
        yield return new WaitForSecondsRealtime(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }

    void Update()
    {
        _tank.GetComponent<Tank_Move>(ComponentType.Move).Move(_moveJoystick.Magnitude);
        _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate).Rotate(_moveJoystick.Direction);
        _tank.Turret.GetComponent<Turret_Rotate>(ComponentType.Rotate).Rotate(_attackJoystick.Direction);
    }
}
