using System.Collections;
using UnityEngine;
using UnityEngine.Events;
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

    private float _cameraHeight = -30;
    public float CameraHeight
    {
        get => _cameraHeight;
        set
        {
            _cameraHeight = Mathf.Clamp(value, -40, -30);
        }
    }


    [Header("Camera Shake")]

    [SerializeField]
    private CameraShakeValueSO _cameraAttackShakeValueSO = null;

    [SerializeField]
    private CameraShakeValueSO _cameraDamageShakeValueSO = null;

    [SerializeField]
    private CameraShakeValueSO _cameraCrashShakeValueSO = null;

    protected override void Awake()
    {
        base.Awake();
        Camera.main.TryGetComponent(out _cameraManager);
        _tank = SpawnManager.Instance.SpawnUnit(PlayerDataManager.Instance.GetPlayerTankID(), transform.position, Quaternion.identity, GroupType.Player);
        _tank.tag = "Player";
        FindObjectOfType<MinimapCameraManager>().Target = _tank.transform;

        _attackJoystick.AddOnPointerUpAction(_tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).Fire);
        _attackJoystick.AddOnPointerUpAction(() => _tank.Turret.GetComponent<Turret_AimLine>(ComponentType.AimLine).SetEnableLineRenderer(false));
        _attackJoystick.AddOnPointerDownAction(() => _tank.Turret.GetComponent<Turret_AimLine>(ComponentType.AimLine).SetEnableLineRenderer(true));
        _hpBar.Setting(_tank.TankData.HP);

        _cameraManager.AddTargetGroup(_tank.transform, 15, 100);

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

        if (shellEquipmentData._shellEquipmentList[0] == "")
        {
            _tank.Turret.CurrentShell = AddressablesManager.Instance.GetResource<GameObject>(shellEquipmentData._shellEquipmentList[1]).GetComponent<Shell>();
        }
        else
        {
            _tank.Turret.CurrentShell = AddressablesManager.Instance.GetResource<GameObject>(shellEquipmentData._shellEquipmentList[0]).GetComponent<Shell>();
        }

        SetTankDamage();

        _tank.GetComponent<Tank_Move>(ComponentType.Move).AddOnCrashAction((a) =>
        {
            _cameraManager.CameraShake(a * 0.3f, _cameraCrashShakeValueSO.FrequencyGain, _cameraCrashShakeValueSO.Duration);
        });

        _tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).AddOnFireAction(() =>
        {
            _cameraManager.CameraShake(_cameraAttackShakeValueSO);
        });

        _tankMove = _tank.GetComponent<Tank_Move>(ComponentType.Move);
        _tankRotate = _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate);
        _turretRotate = _tank.Turret.GetComponent<Turret_Rotate>(ComponentType.Rotate);

        StartCoroutine(nameof(InputUpdateCoroutine));
        // StartCoroutine(nameof(CheckAroundTarget));
    }

    void Update()
    {
        _cameraManager.CameraZoom(_cameraHeight, 1f);
    }

    private IEnumerator InputUpdateCoroutine()
    {
        while (true)
        {
            _tankMove.Move(_moveJoystick.Magnitude);
            _tankRotate.Rotate(_moveJoystick.Direction);
            _turretRotate.Rotate(_attackJoystick.Direction);
            yield return null;
        }
    }

    private IEnumerator CheckAroundTarget()
    {
        bool isChanged = false;

        while (true)
        {
            yield return new WaitForSeconds(1f);
            var findingTank = Physics2D.OverlapCircleAll(_tank.transform.position, 50f, 1 << LayerMask.NameToLayer("Tank"));
            isChanged = _cameraManager.TargetGroupLength != findingTank.Length;
            if (isChanged == true)
            {
                _cameraManager.ResetTargetGroup(findingTank.ToList());
                _cameraHeight = findingTank.Length > 1 ? -40 : -30;
            }

            foreach (var enemy in findingTank)
            {
                _cameraManager.AddTargetGroup(enemy.transform, 20, 80);
            }
        }
    }

    private void SetTankDamage()
    {
        // TODO : 연동이 잘 안되는 경우 존재 해결 필요
        Tank_Damage tankDamage = _tank.GetComponent<Tank_Damage>(ComponentType.Damage);
        tankDamage.AddOnDamageAction(_hpBar.ChangeValue);
        tankDamage.AddOnDamageAction((a) =>
        {
            if (a < 0)
            {
                _cameraManager.CameraShake(_cameraDamageShakeValueSO);
                _cameraManager.SetVolumeVignette(Color.red, 0.25f, 1f, 0.4f);

                object[] objects = new object[2];
                objects[0] = tankDamage.LastHitDir.x;
                objects[1] = tankDamage.LastHitDir.y;
                EventManager.TriggerEvent(EventKeyword.PlayerHit, objects);
            }
        });

        tankDamage.AddOnDeathAction(() =>
        {
            _tank.GetComponent<Tank_Move>(ComponentType.Move).SetEnableMove(false);
            StopCoroutine(nameof(InputUpdateCoroutine));
            _attackJoystick.ClearOnPointerUpAction();
            EventManager.TriggerEvent(EventKeyword.PlayerDead);
            StopCoroutine(nameof(CheckAroundTarget));

#if UNITY_ANDROID
            Handheld.Vibrate();
#endif
        });
    }
}
