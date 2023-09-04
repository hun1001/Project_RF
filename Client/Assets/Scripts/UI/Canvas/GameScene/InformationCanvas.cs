using Event;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationCanvas : BaseCanvas
{
    [Header("Player")]
    [SerializeField]
    private Bar _hpBar = null;
    public Bar HpBar => _hpBar;

    private Player _player;

    [Header("Shell")]
    [SerializeField]
    private ShellToggleGroupManager shellToggleManager = null;
    public ShellToggleGroupManager ShellToggleManager => shellToggleManager;

    [SerializeField]
    private TMP_Text _speedText = null;

    [SerializeField]
    private TankInfoUI _tankInfoUI = null;

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

        _player.TurretAttack.AddOnFireAction(() =>
        {
            shellToggleManager.CurrentCoolDownHandle.SetCoolDown(_player.Tank.Turret.TurretData.ReloadTime);
        });
    }

    private void Update()
    {
        _speedText.text = $"{_player.Tank.GetComponent<Tank_Move>(ComponentType.Move).CurrentSpeed:F1} km/h";

        _tankInfoUI.UpdateTankBodyRotate(_player.Tank.transform.rotation);
        _tankInfoUI.UpdateTankTurretRotate(_player.Tank.Turret.TurretTransform.rotation);
    }
}
