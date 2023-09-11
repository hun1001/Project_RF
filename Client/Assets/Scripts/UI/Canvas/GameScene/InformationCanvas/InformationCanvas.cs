using TMPro;
using UnityEngine;

public class InformationCanvas : BaseCanvas
{
    [Header("Player")]
    [SerializeField]
    private Bar _hpBar = null;
    public Bar HpBar => _hpBar;

    private Player _player;

    [Header("Shell")]
    [SerializeField]
    private ShellToggleGroupSATManager shellToggleManager = null;
    public ShellToggleGroupSATManager ShellToggleManager => shellToggleManager;
    private int _magazineSize = 0;

    [SerializeField]
    private TMP_Text _speedText = null;

    [SerializeField]
    private GameTankInfoUI _tankInfoUI = null;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        _player.OnPlayerDidNotAnyThing += _tankInfoUI.Stop;

        _player.TankRotate.OnTurnLeftAction += _tankInfoUI.TurnLeft;
        _player.TankRotate.OnTurnRightAction += _tankInfoUI.TurnRight;
        _player.TankRotate.OnTurnStopAction += _tankInfoUI.TurnStop;

        _player.TankMove.OnForwardAction += _tankInfoUI.Forward;
        _player.TankMove.OnBackwardAction += _tankInfoUI.Backward;

        if (_player.Tank.Turret.TurretData.IsBurst)
        {
            _magazineSize = _player.Tank.Turret.TurretData.BurstData.MagazineSize;
            _player.TurretAttack.AddOnFireAction(() =>
            {
                if (--_magazineSize <= 0)
                {
                    _magazineSize = _player.Tank.Turret.TurretData.BurstData.MagazineSize;
                    shellToggleManager.SetCoolDown(_player.Tank.Turret.TurretData.ReloadTime);
                }
                else
                {
                    shellToggleManager.SetCoolDown(_player.Tank.Turret.TurretData.BurstData.BurstReloadTime);
                }
            });
        }
        else
        {
            _player.TurretAttack.AddOnFireAction(() =>
            {
                shellToggleManager.SetCoolDown(_player.Tank.Turret.TurretData.ReloadTime);
            });
        }
        
    }

    private void Update()
    {
        _speedText.text = $"{_player.Tank.GetComponent<Tank_Move>(ComponentType.Move).CurrentSpeed:F1} km/h";

        _tankInfoUI.UpdateTankBodyRotate(_player.Tank.transform.rotation);
        _tankInfoUI.UpdateTankTurretRotate(_player.Tank.Turret.TurretTransform.rotation);
    }

    public override void OnOpenEvents()
    {
        base.OnOpenEvents();
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
}