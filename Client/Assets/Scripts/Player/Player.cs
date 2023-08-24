using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Addressable;
using System.Linq;
using Event;
using System;

public class Player : CustomObject
{
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

        MouseManager.Instance.OnMouseLeftButtonDown += _tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).Fire;

        MouseManager.Instance.OnMouseRightButtonUp += () => _tank.Turret.GetComponent<Turret_AimLine>(ComponentType.AimLine).SetEnableLineRenderer(false);
        MouseManager.Instance.OnMouseRightButtonDown += () => _tank.Turret.GetComponent<Turret_AimLine>(ComponentType.AimLine).SetEnableLineRenderer(true);
        _hpBar.Setting(_tank.TankData.HP);

        _cameraManager.AddTargetGroup(_tank.transform, 30, 100);
        var audioListener = _cameraManager.transform.GetChild(1);
        audioListener.SetParent(_tank.transform);
        audioListener.localPosition = Vector3.zero;

        ShellEquipmentData shellEquipmentData = ShellSaveManager.GetShellEquipment(PlayerDataManager.Instance.GetPlayerTankID());
        int shellCnt = shellEquipmentData._shellEquipmentList.Count;
        if (shellEquipmentData._shellEquipmentList.Contains(""))
        {
            shellCnt--;
        }

        for (int i = 0; i < shellEquipmentData._shellEquipmentList.Count; i++)
        {
            int dataIndex = i;
            if (shellEquipmentData._shellEquipmentList[dataIndex] == "")
            {
                continue;
            }

            Shell shell = AddressablesManager.Instance.GetResource<GameObject>(shellEquipmentData._shellEquipmentList[dataIndex]).GetComponent<Shell>();

            _informationCanvas.ShellToggleManager.AddToggle(dataIndex, shell.ID, shell.ShellSprite, (inOn) =>
            {
                if (inOn)
                {
                    _tank.Turret.CurrentShell = shell;
                }
            });

            KeyboardManager.Instance.AddKeyDownAction((KeyCode)((int)KeyCode.Alpha1 + (i)), () =>
            {
                int index = dataIndex;
                _informationCanvas.ShellToggleManager.ToggleList[index].isOn = true;
            });
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
    }

    void Update()
    {
        _cameraManager.CameraZoom(_cameraHeight, 1f);
    }

    private IEnumerator InputUpdateCoroutine()
    {
        while (true)
        {
            Vector2 moveDir = Vector2.zero;
            moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            _tankMove.Move(moveDir.magnitude);
            _tankRotate.Rotate(moveDir.normalized);
            
            _turretRotate.Rotate(MouseManager.Instance.MouseDir);

            if(Input.GetKey(KeyCode.Q)&&Input.GetKey(KeyCode.E))
            {
                _tankMove.Move(-0.1f);
            }
            else if(Input.GetKey(KeyCode.Q))
            {
                _tankRotate.RotateLeft();
            }
            else if(Input.GetKey(KeyCode.E))
            {
                _tankRotate.RotateRight();
            }

            if(Input.GetKey(KeyCode.Space))
            {
                _tankMove.Stop();
            }

            yield return null;
        }
    }

    private void SetTankDamage()
    {
        // TODO : ?�동?????�되??경우 존재 ?�결 ?�요
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
            //_attackJoystick.ClearOnPointerUpAction();
            EventManager.TriggerEvent(EventKeyword.PlayerDead);
        });
    }
}
