using System.Collections;
using UnityEngine;
using Addressable;
using Event;
using System;
using Pool;
using System.Collections.Generic;

public class Player : CustomObject
{
    [SerializeField]
    protected InformationCanvas _informationCanvas = null;

    private Bar _hpBar => _informationCanvas.HpBar;

    private CameraManager _cameraManager = null;

    protected Tank _tank = null;
    public Tank Tank => _tank;

    private Tank_Move _tankMove = null;
    public Tank_Move TankMove => _tankMove;

    private Tank_Rotate _tankRotate = null;
    public Tank_Rotate TankRotate => _tankRotate;

    protected Turret_Rotate _turretRotate = null;
    private Turret_Attack _turretAttack = null;
    public Turret_Attack TurretAttack => _turretAttack;

    private BaseSubArmament _subArmament = null;

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

    public ControlType controlType = ControlType.Detail;

    protected override void Awake()
    {
        base.Awake();
        Camera.main.TryGetComponent(out _cameraManager);

        BaseSubArmament sat = null;

        if (SATSaveManager.SATID != null && SATSaveManager.SATID != string.Empty)
        {
            sat = PoolManager.Get<BaseSubArmament>(SATSaveManager.SATID);
        }

        _tank = SpawnManager.Instance.SpawnUnit(PlayerDataManager.Instance.GetPlayerTankID(), transform.position, Quaternion.identity, GroupType.Player, sat);
        _tank.tag = "Player";

        _tankMove = _tank.GetComponent<Tank_Move>(ComponentType.Move);
        _tankRotate = _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate);
        _turretRotate = _tank.Turret.GetComponent<Turret_Rotate>(ComponentType.Rotate);
        _turretAttack = _tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack);
        
        MouseManager.Instance.OnMouseLeftButtonDown += () =>
        {
            if (Time.timeScale > 0)
            {
                _turretAttack.Fire();
            }
        };

        MouseManager.Instance.OnMouseRightButtonUp += () => _tank.Turret.GetComponent<Turret_AimLine>(ComponentType.AimLine).SetEnableLineRenderer(false);
        MouseManager.Instance.OnMouseRightButtonDown += () => _tank.Turret.GetComponent<Turret_AimLine>(ComponentType.AimLine).SetEnableLineRenderer(true);
        _hpBar.Setting(_tank.TankData.HP);

        _cameraManager.AddTargetGroup(_tank.transform, 30, 100);

        ShellAddInput();

        if (_tank.TryGetSecondaryArmament(out _subArmament))
        {
            _informationCanvas.ShellToggleManager.SetSAT(_subArmament);

            KeyboardManager.Instance.AddKeyDownAction(KeyCode.Space, _subArmament.Aim);
            KeyboardManager.Instance.AddKeyUpAction(KeyCode.Space, _subArmament.StopFire);
        }

        _informationCanvas.ShellToggleManager.TemplateList[0].isOn = true;

        SetTankDamage();

        _tankMove.AddOnCrashAction((a) =>
        {
            _cameraManager.CameraShake(a * 0.3f, _cameraCrashShakeValueSO.FrequencyGain, _cameraCrashShakeValueSO.Duration);
        });

        _turretAttack.AddOnFireAction(() =>
        {
            _cameraManager.CameraShake(_cameraAttackShakeValueSO);
        });

        EventManager.StartListening(EventKeyword.ChangeControlType, (a) => ChangeControlType(a));

        int controlTypeSetting = PlayerPrefs.GetInt("ControlType", 0);
        controlType = (ControlType)controlTypeSetting;
        
        StartCoroutine(nameof(InputUpdateCoroutine));
    }

    protected bool _wasControlled = true;

    private void ChangeControlType(object[] obj)
    {
        int controlTypeSetting = (int)obj[0];
        controlType = (ControlType)controlTypeSetting;
    }

    protected virtual IEnumerator InputUpdateCoroutine()
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

    protected void DetailControl()
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

    protected void SimpleControl()
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

    protected virtual void ShellAddInput()
    {
        ShellEquipmentData shellEquipmentData = ShellSaveManager.GetShellEquipment(PlayerDataManager.Instance.GetPlayerTankID());
        int shellCnt = 0;

        List<Action> actionList = new List<Action>();
        for (int i = 0; i < shellEquipmentData._shellEquipmentList.Count; i++)
        {
            int dataIndex = i;
            if (shellEquipmentData._shellEquipmentList[dataIndex] == "")
            {
                shellCnt++;
                continue;
            }
            int emptyCnt = shellCnt;
            Shell shell = AddressablesManager.Instance.GetResource<GameObject>(shellEquipmentData._shellEquipmentList[dataIndex]).GetComponent<Shell>();

            _informationCanvas.ShellToggleManager.AddToggle(dataIndex - shellCnt, shell.ID, shell.ShellSprite, (inOn) =>
            {
                if (inOn)
                {
                    _tank.Turret.CurrentShell = shell;
                }
            });

            actionList.Add(() =>
            {
                int index = dataIndex;
                int cnt = emptyCnt;
                _informationCanvas.ShellToggleManager.TemplateList[index - cnt].isOn = true;
            });
        }
        Action[] actions = actionList.ToArray();
        KeyboardManager.Instance.AddKeyDownActionList(actions);

        if (shellEquipmentData._shellEquipmentList[0] == "")
        {
            _tank.Turret.CurrentShell = AddressablesManager.Instance.GetResource<GameObject>(shellEquipmentData._shellEquipmentList[1]).GetComponent<Shell>();
        }
        else
        {
            _tank.Turret.CurrentShell = AddressablesManager.Instance.GetResource<GameObject>(shellEquipmentData._shellEquipmentList[0]).GetComponent<Shell>();
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
            StartCoroutine(nameof(DeathEffectCoroutine));
            EventManager.TriggerEvent(EventKeyword.PlayerDead);
        });
    }

    private IEnumerator DeathEffectCoroutine()
    {
        yield return new WaitForSeconds(1);
        PoolManager.Get("TankDeathEffect", Tank.transform.position, Quaternion.Euler(0, 0, 0));
    }
}
