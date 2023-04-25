using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : CustomObject
{
    [Header("Canvas")]
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

    private Tank_Move _tankMove = null;
    private Tank_Rotate _tankRotate = null;
    private Turret_Rotate _turretRotate = null;

    [Header("Camera Shake")]
    public float cameraAttackShakeAmplitudeGain = 4f;
    public float cameraAttackShakeFrequencyGain = 6;
    public float cameraAttackShakeDuration = 0.1f;

    [Space(10f)]
    public float cameraDamageShakeAmplitudeGain = 5f;
    public float cameraDamageShakeFrequencyGain = 8;
    public float cameraDamageShakeDuration = 0.2f;

    [Space(10f)]
    public float cameraCrashShakeFrequencyGain = 8;
    public float cameraCrashShakeDuration = 0.2f;

    protected override void Awake()
    {
        base.Awake();
        Camera.main.TryGetComponent(out _cameraManager);
        _tank = SpawnManager.Instance.SpawnUnit(PlayerDataManager.Instance.GetPlayerTankID(), Vector3.zero, Quaternion.identity, GroupType.Player);
        _tank.tag = "Player";
        MinimapCameraManager.Instance.Target = _tank.transform;
        _cameraManager.SetPlayer(_tank.transform);
        _attackJoystick.AddOnPointerUpAction(_tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).Fire);
        _hpBar.Setting(_tank.TankData.HP);

        if (_tank.TankData.HasSkill == true)
        {
            _tank.GetComponent<Tank_Skill>(ComponentType.Skill).SkillImage = _controllerCanvas.ButtonGroup.transform.GetChild(0).GetComponent<Image>();
            _controllerCanvas.ButtonGroup.SetButton(0, _tank.GetComponent<Tank_Skill>(ComponentType.Skill).UseSkill);
        }
        else
        {
            _controllerCanvas.ButtonGroup.SetButton(0, null, false);
        }

        _controllerCanvas.ButtonGroup.SetButton(1, null, false);
        _controllerCanvas.ButtonGroup.SetButton(2, null, false);

        int shellCnt = _tank.Turret.TurretData.Shells.Count;

        string[] shellName = new string[shellCnt];
        Sprite[] shellSprite = new Sprite[shellCnt];
        UnityAction<bool>[] shellAction = new UnityAction<bool>[shellCnt];

        for (int i = 0; i < shellCnt; i++)
        {
            int index = i;
            shellName[index] = _tank.Turret.TurretData.Shells[index].ID;
            shellSprite[index] = _tank.Turret.TurretData.Shells[index].ShellSO.ShellSprite;
            shellAction[index] = (_isOn) =>
            {
                if (_isOn)
                {
                    _tank.Turret.CurrentShell = _tank.Turret.TurretData.Shells[index];
                }
                else
                {

                }
            };
        }

        _controllerCanvas.ToggleGroup.SetToggleGroup(shellName, shellSprite, shellAction);

        // TODO : 연동이 잘 안되는 경우 존재 해결 필요
        _tank.GetComponent<Tank_Damage>(ComponentType.Damage).AddOnDamageAction(_hpBar.ChangeValue);
        _tank.GetComponent<Tank_Damage>(ComponentType.Damage).AddOnDamageAction((a) =>
        {
            if (a < 0) _cameraManager.CameraShake(cameraDamageShakeAmplitudeGain, cameraDamageShakeFrequencyGain, cameraDamageShakeDuration);

            if (ServerManager.Instance.IsPlayingGame)
            {
                for (int i = 0; i < 25; ++i)
                {
                    ServerManager.Instance.SendHP(_tank.GetComponent<Tank_Damage>(ComponentType.Damage).CurrentHealth);
                }
            }
        });

        _tank.GetComponent<Tank_Damage>(ComponentType.Damage).AddOnDeathAction(() =>
        {
            Time.timeScale = 0;
            ServerManager.Instance.Disconnect();
            StartCoroutine(Change());
        });

        _tank.GetComponent<Tank_Move>(ComponentType.Move).AddOnCrashAction((a) =>
        {
            _cameraManager.CameraShake(a * 0.4f, cameraCrashShakeFrequencyGain, cameraCrashShakeDuration);
        });

        _tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).AddOnFireAction(() =>
        {
            _cameraManager.CameraShake(cameraAttackShakeAmplitudeGain, cameraAttackShakeFrequencyGain, cameraAttackShakeDuration);
            if (ServerManager.Instance.IsPlayingGame)
            {
                for (int i = 0; i < 25; ++i)
                {
                    ServerManager.Instance.AttackPlayer();
                }
            }
        });

        _tankMove = _tank.GetComponent<Tank_Move>(ComponentType.Move);
        _tankRotate = _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate);
        _turretRotate = _tank.Turret.GetComponent<Turret_Rotate>(ComponentType.Rotate);

        StartCoroutine(UpdateServerTransform());
    }

    void Update()
    {
        _tankMove.Move(_moveJoystick.Magnitude);
        _tankRotate.Rotate(_moveJoystick.Direction);
        _turretRotate.Rotate(_attackJoystick.Direction);
    }

    private IEnumerator UpdateServerTransform()
    {
        while (true)
        {
            if (ServerManager.Instance.IsPlayingGame)
            {
                ServerManager.Instance.SendTransform(_tank.transform, _tank.Turret.TurretTransform);
            }

            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private IEnumerator Change()
    {
        yield return new WaitForSecondsRealtime(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }
}
