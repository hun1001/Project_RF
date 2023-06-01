using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Event;
using Addressable;

public class ShellReplacement : MonoBehaviour
{
    [SerializeField]
    private GameObject _template;
    [SerializeField]
    private RectTransform _parent;

    private string _currentTankID = string.Empty;
    private ShellEquipmentData _shellEquipmentData = null;
    private Dictionary<string, GameObject> _shellDict = new Dictionary<string, GameObject>();
    private List<Shell> _shells = new List<Shell>();

    private void Start()
    {
        
    }

    private void ShellReset()
    {
        for (int i = 1; i < _parent.childCount; i++)
        {
            _parent.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void ShellAdd()
    {
        _currentTankID = PlayerDataManager.Instance.GetPlayerTankID();
        _shells = AddressablesManager.Instance.GetResource<GameObject>(_currentTankID).GetComponent<Turret>().TurretSO.Shells;
        _shellEquipmentData = ShellSaveManager.GetShellEquipment(_currentTankID);

        for (int i = 0; i < _shells.Count; i++)
        {
            if (_shellDict.ContainsKey(_shells[i].ID))
            {
                _shellDict[_shells[i].ID].SetActive(true);

                if (_shellEquipmentData._shellEquipmentList.Contains(_shells[i].ID))
                {
                    _shellDict[_shells[i].ID].transform.GetChild(4).GetComponent<Toggle>().isOn = true;
                }
                else
                {
                    _shellDict[_shells[i].ID].transform.GetChild(4).GetComponent<Toggle>().isOn = false;
                }

                continue;
            }

            var obj = Instantiate(_template, _parent);
            _shellDict.Add(_shells[i].ID, obj);
            obj.SetActive(true);

            obj.transform.GetChild(0).GetComponent<Image>().sprite = _shells[i].ShellSprite;
            Transform information = obj.transform.GetChild(1);
            information.GetChild(0).GetComponent<TextController>().SetText(_shells[i].ID);

            // Bar
            // Value Text

            if (_shellEquipmentData._shellEquipmentList.Contains(_shells[i].ID))
            {
                _shellDict[_shells[i].ID].transform.GetChild(4).GetComponent<Toggle>().isOn = true;
            }
            else
            {
                _shellDict[_shells[i].ID].transform.GetChild(4).GetComponent<Toggle>().isOn = false;
            }
        }
    }
}
