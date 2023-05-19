using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControllerCanvas : BaseCanvas
{
    [SerializeField]
    private Joystick _moveJoystick = null;
    public Joystick MoveJoystick => _moveJoystick;

    [SerializeField]
    private Joystick _attackJoystick = null;
    public Joystick AttackJoystick => _attackJoystick;

    [SerializeField]
    private ButtonGroupManager _buttonGroup = null;
    public ButtonGroupManager ButtonGroup => _buttonGroup;

    [SerializeField]
    private ToggleGroupManager _toggleGroup = null;
    public ToggleGroupManager ToggleGroup => _toggleGroup;

    [SerializeField]
    private Image _reloadImage = null;

    private Player _player;
    private Turret _playerTurret;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        Tank pt = _player.Tank;
        _playerTurret = pt.Turret;
        _playerTurret.GetComponent<Turret_Attack>().AddOnFireAction(() => StartCoroutine(ReloadCheck()));
    }

    private IEnumerator ReloadCheck()
    {
        float reloadTime = 0;
        float currentTime = 0;

        if (_playerTurret.TurretSO.IsBurst)
        {
            int currentMagazine = _playerTurret.GetComponent<Turret_Attack>().MagazineSize;

            if (currentMagazine > 0)
            {
                reloadTime = _playerTurret.TurretSO.BurstData.BurstReloadTime;
            }
            else
            {
                reloadTime = _playerTurret.TurretSO.ReloadTime;
            }
        }
        else
        {
            reloadTime = _playerTurret.TurretSO.ReloadTime;
        }

        while (currentTime < reloadTime)
        {
            _reloadImage.fillAmount = currentTime / reloadTime;
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
