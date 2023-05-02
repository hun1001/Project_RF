using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Addressable;
using Pool;
using System.Linq;

public class TrainingTankManager : MonoBehaviour
{
    [SerializeField]
    private Transform _tankTransform = null;

    [SerializeField]
    private Dropdown _tankDropdown = null;

    [SerializeField]
    private Text _debugText = null;

    private StringBuilder _debugTextString = new StringBuilder();

    private List<GameObject> _tankList = new List<GameObject>();

    private Tank _tank = null;

    private Tank _playerTank = null;

    private void Awake()
    {
        _tankList = AddressablesManager.Instance.GetLabelResources<GameObject>("Tank").ToList();

        _tankDropdown.ClearOptions();

        _tankDropdown.AddOptions(_tankList.Select(t => t.name).ToList());

        _tankDropdown.onValueChanged.AddListener(OnDropboxValueChanged);
        OnDropboxValueChanged(0);
    }

    private void OnDropboxValueChanged(int index)
    {
        if (_tank != null)
        {
            PoolManager.Pool(_tank.ID, _tank.gameObject);
        }

        _tank = SpawnManager.Instance.SpawnUnit(_tankList[index].name, _tankTransform.position, _tankTransform.rotation, GroupType.Enemy);

        _tank.GetComponent<Tank_Damage>().AddOnDamageAction(SetDebugText);
        SetDebugText(0);
    }

    private void SetDebugText(float d)
    {
        _playerTank ??= FindObjectOfType<Player>().Tank;
        _debugTextString.Clear();
        _debugTextString.Append($"Attack Damage: {Mathf.Round(_playerTank.Turret.CurrentShell.ShellSO.Damage * Mathf.Pow(_playerTank.Turret.TurretData.AtkPower, 2) * 0.001f)}\n");
        _debugTextString.Append($"Penetration: {Mathf.Round(_playerTank.Turret.TurretData.AtkPower * _playerTank.Turret.TurretData.PenetrationPower * _playerTank.Turret.CurrentShell.ShellSO.Penetration / 3000f)}\n");
        _debugTextString.Append($"Target Amour: {_tank.TankData.Armour}\n");
        _debugTextString.Append($"Result Damage: {-d}\n");

        _debugText.text = _debugTextString.ToString();
    }
}
