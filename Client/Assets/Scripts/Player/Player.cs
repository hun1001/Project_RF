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
    public Tank_Move TankMove => _tankMove;

    private Tank_Rotate _tankRotate = null;
    public Tank_Rotate TankRotate => _tankRotate;

    private Turret_Rotate _turretRotate = null;
    private Turret_Attack _turretAttack = null;
    public Turret_Attack TurretAttack => _turretAttack;

    private SubArmament _subArmament = null;

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

    public Action OnPlayerDidNotAnyThing = null;

    public ControlType controlType = ControlType.Simple;

    protected override void Awake()
    {
        base.Awake();
        Camera.main.TryGetComponent(out _cameraManager);
        _tank = SpawnManager.Instance.SpawnUnit(PlayerDataManager.Instance.GetPlayerTankID(), transform.position, Quaternion.identity, GroupType.Player);
        _tank.tag = "Player";

        _tankMove = _tank.GetComponent<Tank_Move>(ComponentType.Move);
        _tankRotate = _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate);
        _turretRotate = _tank.Turret.GetComponent<Turret_Rotate>(ComponentType.Rotate);
        _turretAttack = _tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack);
        
        if(_tank.TryGetSecondaryArmament(out _subArmament))
        {
            if (_subArmament.ActionType == SubArmamentKeyActionType.OnKeyHold)
            {
                KeyboardManager.Instance.AddKeyHoldAction(KeyCode.Space, _subArmament.Fire);
            }
            else if (_subArmament.ActionType == SubArmamentKeyActionType.OnKeyDownUp)
            {
                
                KeyboardManager.Instance.AddKeyUpAction(KeyCode.Space, _subArmament.Fire);
            }
        }

        MouseManager.Instance.OnMouseLeftButtonDown += _turretAttack.Fire;

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
                _informationCanvas.ShellToggleManager.TemplateList[index].isOn = true;
            });
        }

        _informationCanvas.ShellToggleManager.TemplateList[0].isOn = true;

        if (shellEquipmentData._shellEquipmentList[0] == "")
        {
            _tank.Turret.CurrentShell = AddressablesManager.Instance.GetResource<GameObject>(shellEquipmentData._shellEquipmentList[1]).GetComponent<Shell>();
        }
        else
        {
            _tank.Turret.CurrentShell = AddressablesManager.Instance.GetResource<GameObject>(shellEquipmentData._shellEquipmentList[0]).GetComponent<Shell>();
        }

        SetTankDamage();

        _tankMove.AddOnCrashAction((a) =>
        {
            _cameraManager.CameraShake(a * 0.3f, _cameraCrashShakeValueSO.FrequencyGain, _cameraCrashShakeValueSO.Duration);
        });

        _turretAttack.AddOnFireAction(() =>
        {
            _cameraManager.CameraShake(_cameraAttackShakeValueSO);
        });        

        StartCoroutine(nameof(InputUpdateCoroutine));
    }

    void Update()
    {
        _cameraManager.CameraZoom(_cameraHeight, 1f);
    }

    private bool _wasControlled = true;

    private IEnumerator InputUpdateCoroutine()
    {
        while (true)
        {
            _turretRotate.Rotate(MouseManager.Instance.MouseDir);

            switch (controlType)
            {
                case ControlType.Detail:
                    DetailControl();
                    break;
                case ControlType.Simple:
                    SimpleControl();
                    break;
            }

            if (!_wasControlled)
            {
                OnPlayerDidNotAnyThing?.Invoke();
            }

            yield return null;
        }
    }

    private void DetailControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _tankMove.Move(1f);
            _wasControlled = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _tankMove.Move(-0.4f);
            _wasControlled = true;
        }
        else
        {
            _tankMove.Move(0f);
            _wasControlled = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _tankRotate.RotateRight();
            _wasControlled = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _tankRotate.RotateLeft();
            _wasControlled = true;
        }
        else
        {
            _wasControlled = _wasControlled || false;
        }

        if(Input.GetKeyUp(KeyCode.A)||Input.GetKeyUp(KeyCode.D)||Input.GetKeyUp(KeyCode.W)||Input.GetKeyUp(KeyCode.S))
        {
            OnPlayerDidNotAnyThing?.Invoke();
        }
    }

    private void SimpleControl()
    {
        Vector2 moveDir = Vector2.zero;
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _tankMove.Move(moveDir.magnitude);
        _tankRotate.Rotate(moveDir.normalized);

        _wasControlled = moveDir != Vector2.zero;

        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
        {
            _tankMove.Move(-0.4f);
            _tankRotate.OnTurnStopAction?.Invoke();
            _wasControlled = true;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            _tankRotate.RotateLeft();
            _wasControlled = true;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            _tankRotate.RotateRight();
            _wasControlled = true;
        }
        else
        {
            _wasControlled = _wasControlled || false;
        }

        if (Input.GetKey(KeyCode.F))
        {
            _tankMove.Stop();
            _wasControlled = false;
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
