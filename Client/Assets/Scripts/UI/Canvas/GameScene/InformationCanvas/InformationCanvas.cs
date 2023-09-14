using Event;
using TMPro;
using UnityEngine;

public class InformationCanvas : BaseCanvas
{
    [Header("Player")]
    [SerializeField]
    private Bar _hpBar = null;
    public Bar HpBar => _hpBar;

    private Player _player;

    [SerializeField]
    private TextController _speedText = null;

    [SerializeField]
    private GameTankInfoUI _tankInfoUI = null;

    [Header("Shell")]
    [SerializeField]
    private ShellToggleGroupSATManager shellToggleManager = null;
    public ShellToggleGroupSATManager ShellToggleManager => shellToggleManager;
    private int _magazineSize = 0;

    [Header("Rader")]
    [SerializeField]
    private EnemyPositionArrowGroupHandle _enemyPositionArrowGroupHandle = null;

    [Header("Wave")]
    [SerializeField]
    private TextController _waveText = null;
    [SerializeField]
    private TextController _enemyText = null;

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

        _enemyText.SetText(GameWay_Base.RemainingEnemy);
        EventManager.StartListening(EventKeyword.EnemyDie, () =>
        {
            int enemy = GameWay_Base.RemainingEnemy;
            if (enemy > 0)
            {
                _enemyText.SetText(enemy);
            }
            else
            {
                _enemyText.SetText("Clear");
            }
        });

        EventManager.StartListening("Clear", () =>
        {
            _waveText.SetText("Wave " + (GameWay_Base.CurrentStage + 1).ToString());
            _enemyText.SetText(GameWay_Base.RemainingEnemy);
        });
    }

    private void Update()
    {
        _speedText.SetText($"{_player.Tank.GetComponent<Tank_Move>(ComponentType.Move).CurrentSpeed:F1} km/h");

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
