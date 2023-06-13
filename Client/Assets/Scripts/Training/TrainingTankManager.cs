using System.Text;
using System.Collections;
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

    [SerializeField]
    private float _destroyedTankPoolingTime = 0f;

    private StringBuilder _debugTextString = new StringBuilder();

    private List<GameObject> _tankList = new List<GameObject>();

    private Tank _tank = null;
    private Bar _hpBar = null;

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
            PoolManager.Pool("EnemyBar", _hpBar.gameObject);
        }

        _tank = SpawnManager.Instance.SpawnUnit(_tankList[index].name, _tankTransform.position, _tankTransform.rotation, GroupType.Enemy);
        _hpBar = PoolManager.Get<Bar>("EnemyBar", _tank.transform.position, Quaternion.identity, _tank.transform);
        _hpBar.Setting(_tank.TankData.HP);

        _tank.GetComponent<Tank_Damage>().AddOnDamageAction(SetDebugText);
        _tank.GetComponent<Tank_Damage>().AddOnDamageAction(_hpBar.ChangeValue);
        _tank.GetComponent<Tank_Damage>().AddOnDeathAction(() => StartCoroutine(DestroyedTankPool()));

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

    private IEnumerator DestroyedTankPool()
    {
        GameObject obj = GameObject.Find("Destroyed_Tank(Clone)");
        string id = obj.name;

        if (id.Contains("(Clone)"))
        {
            id = id.Replace("(Clone)", "");
        }

        yield return new WaitForSeconds(_destroyedTankPoolingTime);
        
        PoolManager.Pool(id, obj);
    }
}
