using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Addressable;
using System.Linq;
using Event;

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
        _tank = SpawnManager.Instance.SpawnUnit(PlayerDataManager.Instance.GetPlayerTankID(), transform.position, Quaternion.identity, GroupType.Player);
        _tank.tag = "Player";
        FindObjectOfType<MinimapCameraManager>().Target = _tank.transform;
        //_cameraManager.SetPlayer(_tank.transform);
        _attackJoystick.AddOnPointerUpAction(_tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).Fire);
        _attackJoystick.AddOnPointerUpAction(() => _tank.Turret.GetComponent<Turret_AimLine>(ComponentType.AimLine).SetEnableLineLenderer(false));
        _attackJoystick.AddOnPointerDownAction(() => _tank.Turret.GetComponent<Turret_AimLine>(ComponentType.AimLine).SetEnableLineLenderer(true));
        _hpBar.Setting(_tank.TankData.HP);

        _cameraManager.AddTargetGroup(_tank.transform, 15, 100);

        //if (_tank.TankData.HasSkill == true)
        //{
        //    _tank.GetComponent<Tank_Skill>(ComponentType.Skill).SkillImage = _controllerCanvas.ButtonGroup.transform.GetChild(0).GetComponent<Image>();
        //    _controllerCanvas.ButtonGroup.SetButton(0, _tank.GetComponent<Tank_Skill>(ComponentType.Skill).UseSkill);
        //}
        //else
        //{
        //    _controllerCanvas.ButtonGroup.SetButton(0, null, null, false);
        //}

        ShellEquipmentData shellEquipmentData = ShellSaveManager.GetShellEquipment(PlayerDataManager.Instance.GetPlayerTankID());
        int shellCnt = shellEquipmentData._shellEquipmentList.Count;
        if (shellEquipmentData._shellEquipmentList.Contains(""))
        {
            shellCnt--;
        }

        string[] shellName = new string[shellCnt];
        Sprite[] shellSprite = new Sprite[shellCnt];
        UnityAction[] shellAction = new UnityAction[shellCnt];

        int slotIndex = 0;
        for (int i = 0; i < shellEquipmentData._shellEquipmentList.Count; i++)
        {
            int dataIndex = i;
            if (shellEquipmentData._shellEquipmentList[dataIndex] == "")
            {
                _controllerCanvas.ButtonGroup.SetButton(slotIndex, null, null, false);
                continue;
            }

            Shell shell = AddressablesManager.Instance.GetResource<GameObject>(shellEquipmentData._shellEquipmentList[dataIndex]).GetComponent<Shell>();
            shellName[slotIndex] = shell.ID;
            shellSprite[slotIndex] = shell.ShellSprite;
            shellAction[slotIndex] = () =>
            {
                _tank.Turret.CurrentShell = shell;
            };
            _controllerCanvas.ButtonGroup.SetButton(slotIndex, shellAction[slotIndex], shellSprite[slotIndex]);
            slotIndex++;
        }

        // TODO : 연동이 잘 안되는 경우 존재 해결 필요
        Tank_Damage tankDamage = _tank.GetComponent<Tank_Damage>(ComponentType.Damage);
        tankDamage.AddOnDamageAction(_hpBar.ChangeValue);
        tankDamage.AddOnDamageAction((a) =>
        {
            if (a < 0)
            {
                _cameraManager.CameraShake(cameraDamageShakeAmplitudeGain, cameraDamageShakeFrequencyGain, cameraDamageShakeDuration);
                _cameraManager.SetVolumeVignette(Color.red, 0.25f, 1f, 0.4f);

                object[] objects = new object[2];
                objects[0] = tankDamage.LastHitDir.x;
                objects[1] = tankDamage.LastHitDir.y;
                EventManager.TriggerEvent(EventKeyword.PlayerHit, objects);
            }
        });


        tankDamage.AddOnDeathAction(() =>
        {
            EventManager.TriggerEvent(EventKeyword.PlayerDead);
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

        StartCoroutine(CheckAroundTarget());
    }

    void Update()
    {
        _tankMove.Move(_moveJoystick.Magnitude);
        _tankRotate.Rotate(_moveJoystick.Direction);
        _turretRotate.Rotate(_attackJoystick.Direction);

        if (_attackJoystick.DragTime > 3f && _tankMove.CurrentSpeed == 0)
        {
            _cameraManager.CameraZoom(-50, 2f);
        }
        else
        {
            _cameraManager.CameraZoom(-30f, 0.3f);
        }
    }

    private IEnumerator CheckAroundTarget()
    {
        bool isChanged = false;

        while (true)
        {
            yield return new WaitForSeconds(1f);
            var findingTank = Physics2D.OverlapCircleAll(_tank.transform.position, 60f, 1 << LayerMask.NameToLayer("Tank"));
            isChanged = _cameraManager.TargetGroupLength != findingTank.Length;
            if (isChanged == true)
            {
                _cameraManager.ResetTargetGroup(findingTank.ToList());
            }

            foreach (var enemy in findingTank)
            {
                _cameraManager.AddTargetGroup(enemy.transform, 20, 80);
            }
        }
    }
}
