using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using UnityEngine.Events;

public class Player : CustomObject
{
    [SerializeField]
    private ControllerCanvas _controllerCanvas = null;

    private Joystick _moveJoystick => _controllerCanvas.MoveJoystick;
    private Joystick _attackJoystick => _controllerCanvas.AttackJoystick;

    [SerializeField]
    private InformationCanvas _informationCanvas = null;

    private Bar _hpBar => _informationCanvas.HpBar;

    private CameraManager _cameraManager = null;

    private Tank _tank = null;
    public Tank Tank => _tank;

    protected override void Awake()
    {
        base.Awake();
        _cameraManager = Camera.main.GetComponent<CameraManager>();

        _tank = PoolManager.Get<Tank>("T-44").SetGroupType(GroupType.Player);
        PoolManager.Get("MinimapCamera", _tank.transform);
        _tank.tag = "Player";

        _cameraManager.SetPlayer(_tank.transform);
        _attackJoystick.AddOnPointerUpAction(_tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).Fire);
        _hpBar.Setting(_tank.TankData.HP);

        int shellCnt = _tank.Turret.TurretData.Shells.Count;

        string[] shellName = new string[shellCnt];
        UnityAction<bool>[] shellAction = new UnityAction<bool>[shellCnt];

        for (int i = 0; i < shellCnt; i++)
        {
            shellName[i] = _tank.Turret.TurretData.Shells[i].ID;
            shellAction[i] = (_isOn) => ToggleChangeShellEvent(_isOn, i);
        }

        _controllerCanvas.ToggleGroup.SetToggleGroup(shellName, shellAction);

        // TODO : 연동이 잘 안되는 경우 존재 해결 필요
        _tank.GetComponent<Tank_Damage>(ComponentType.Damage).AddOnDamageAction(_hpBar.ChangeValue);
        _tank.GetComponent<Tank_Damage>(ComponentType.Damage).AddOnDamageAction((a) => _cameraManager.CameraShake(5f, 8, 0.2f));
        _tank.GetComponent<Tank_Damage>(ComponentType.Damage).AddOnDeathAction(() =>
        {
            Time.timeScale = 0;
            StartCoroutine(Change());
        });

        _tank.Turret.CurrentShellID = "APHE";

        _tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).AddOnFireAction(() => _cameraManager.CameraZoomInEffect(5f, 0.1f, 0.1f));
    }

    void Update()
    {
        _tank.GetComponent<Tank_Move>(ComponentType.Move).Move(_moveJoystick.Magnitude);
        _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate).Rotate(_moveJoystick.Direction);
        _tank.Turret.GetComponent<Turret_Rotate>(ComponentType.Rotate).Rotate(_attackJoystick.Direction);
    }

    private IEnumerator Change()
    {
        yield return new WaitForSecondsRealtime(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }

    private void ToggleChangeShellEvent(bool isOn, int shellIndex)
    {
        Debug.Log(shellIndex);
    }
}
