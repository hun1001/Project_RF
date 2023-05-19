using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cinemachine;
using Addressable;

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
        FindObjectOfType<MinimapCameraManager>().Target = _tank.transform;
        _cameraManager.SetPlayer(_tank.transform);
        _attackJoystick.AddOnPointerUpAction(_tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).Fire);
        _attackJoystick.AddOnPointerUpAction(() => _tank.Turret.GetComponent<Turret_AimLine>(ComponentType.AimLine).SetEnableLineLenderer(false));
        _attackJoystick.AddOnPointerDownAction(() => _tank.Turret.GetComponent<Turret_AimLine>(ComponentType.AimLine).SetEnableLineLenderer(true));
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

        ShellEquipmentData shellEquipmentData = ShellSaveManager.GetShellEquipment(PlayerDataManager.Instance.GetPlayerTankID());
        int shellCnt = shellEquipmentData._shellEquipmentList.Count;
        if (shellEquipmentData._shellEquipmentList.Contains(""))
        {
            shellCnt--;
        }

        string[] shellName = new string[shellCnt];
        Sprite[] shellSprite = new Sprite[shellCnt];
        UnityAction<bool>[] shellAction = new UnityAction<bool>[shellCnt];

        int slotIndex = 0;
        for (int i = 0; i < shellEquipmentData._shellEquipmentList.Count; i++)
        {
            int dataIndex = i;
            if (shellEquipmentData._shellEquipmentList[dataIndex] == "")
            {
                continue;
            }

            Shell shell = AddressablesManager.Instance.GetResource<GameObject>(shellEquipmentData._shellEquipmentList[dataIndex]).GetComponent<Shell>();
            shellName[slotIndex] = shell.ID;
            shellSprite[slotIndex] = shell.ShellSprite;
            shellAction[slotIndex] = (_isOn) =>
            {
                if (_isOn)
                {
                    _tank.Turret.CurrentShell = shell;
                }
                else
                {

                }
            };
            slotIndex++;
        }

        _controllerCanvas.ToggleGroup.SetToggleGroup(shellName, shellSprite, shellAction);

        // TODO : 연동이 잘 안되는 경우 존재 해결 필요
        _tank.GetComponent<Tank_Damage>(ComponentType.Damage).AddOnDamageAction(_hpBar.ChangeValue);
        _tank.GetComponent<Tank_Damage>(ComponentType.Damage).AddOnDamageAction((a) =>
        {
            if (a < 0) _cameraManager.CameraShake(cameraDamageShakeAmplitudeGain, cameraDamageShakeFrequencyGain, cameraDamageShakeDuration);
        });

        _tank.GetComponent<Tank_Damage>(ComponentType.Damage).AddOnDeathAction(() =>
        {
            Event.EventManager.TriggerEvent(EventKeyword.PlayerDead);
        });

        _tank.GetComponent<Tank_Move>(ComponentType.Move).AddOnCrashAction((a) =>
        {
            _cameraManager.CameraShake(a * 0.3f, cameraCrashShakeFrequencyGain, cameraCrashShakeDuration);
        });

        _tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).AddOnFireAction(() =>
        {
            _cameraManager.CameraShake(cameraAttackShakeAmplitudeGain, cameraAttackShakeFrequencyGain, cameraAttackShakeDuration);
        });

        _tankMove = _tank.GetComponent<Tank_Move>(ComponentType.Move);
        _tankRotate = _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate);
        _turretRotate = _tank.Turret.GetComponent<Turret_Rotate>(ComponentType.Rotate);
    }

    void Update()
    {
        _tankMove.Move(_moveJoystick.Magnitude);
        _tankRotate.Rotate(_moveJoystick.Direction);
        _turretRotate.Rotate(_attackJoystick.Direction);

        if (_attackJoystick.DragTime > 1.5f)
        {
            _cameraManager.CameraZoom(-50, 1);
        }
        else
        {
            _cameraManager.CameraZoom(-30f, 0.3f);
        }
    }
}
