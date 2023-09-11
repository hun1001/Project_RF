using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Event;
using Addressable;
using Pool;
using DG.Tweening;
using TMPro;

public class ShellReplacement : MonoBehaviour, IButtonSound
{
    [Header("Sound")]
    [SerializeField]
    private AudioClip _buttonSound = null;

    [Header("Shell")]
    [SerializeField]
    private GameObject _template;
    [SerializeField]
    private RectTransform _parent;

    [Header("Warning")]
    [SerializeField]
    private RectTransform _warningPanel;
    private Sequence _warningSequence;

    private string _currentTankID = string.Empty;
    private ShellEquipmentData _shellEquipmentData = null;
    private Dictionary<string, GameObject> _shellDict = new Dictionary<string, GameObject>();
    private Dictionary<int, Shell> _shellEquipDict = new Dictionary<int, Shell>();
    private List<Shell> _shells = new List<Shell>();

    private void Start()
    {
        _template.SetActive(false);
        ShellAdd();

        EventManager.StartListening(EventKeyword.TankReplacement, () =>
        {
            ShellReset();
            ShellAdd();
        });
    }

    private void ShellReset()
    {
        _shellEquipDict.Clear();
        for (int i = 1; i < _parent.childCount; i++)
        {
            _parent.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void ShellAdd()
    {
        _currentTankID = PlayerDataManager.Instance.GetPlayerTankID();
        Turret turret = AddressablesManager.Instance.GetResource<GameObject>(_currentTankID).GetComponent<Turret>();
        _shells = turret.TurretSO.Shells;
        _shellEquipmentData = ShellSaveManager.GetShellEquipment(_currentTankID);

        for (int i = 0; i < _shells.Count; i++)
        {
            int idx = i;
            float dmg = Mathf.Round(_shells[i].ShellSO.Damage * (Mathf.Pow(turret.TurretSO.AtkPower, 2) * 0.001f));
            float pen = Mathf.Round(turret.TurretSO.AtkPower * turret.TurretSO.PenetrationPower * _shells[i].ShellSO.Penetration / 3000f);

            if (_shellDict.ContainsKey(_shells[idx].ID))
            {
                _shellDict[_shells[idx].ID].SetActive(true);

                _shellDict[_shells[idx].ID].transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = dmg / 1500f;
                _shellDict[_shells[idx].ID].transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = pen / 500f;

                _shellDict[_shells[idx].ID].transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = dmg.ToString();
                _shellDict[_shells[idx].ID].transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = pen.ToString();

                if (_shellEquipmentData._shellEquipmentList.Contains(_shells[i].ID))
                {
                    _shellDict[_shells[idx].ID].transform.GetChild(4).GetComponent<Toggle>().isOn = true;
                    _shellEquipDict.Add(_shellEquipmentData._shellEquipmentList.IndexOf(_shells[idx].ID), _shells[idx]);
                }
                else
                {
                    _shellDict[_shells[i].ID].transform.GetChild(4).GetComponent<Toggle>().isOn = false;
                }

                continue;
            }

            var obj = Instantiate(_template, _parent);
            _shellDict.Add(_shells[idx].ID, obj);
            obj.SetActive(true);

            obj.transform.GetChild(0).GetComponent<Image>().sprite = _shells[idx].ShellSprite;
            Transform informations = obj.transform.GetChild(1);
            informations.GetChild(0).GetComponent<TextController>().SetText(_shells[idx].ID);

            // Bar
            Transform bars = obj.transform.GetChild(2);
            bars.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = dmg / 1500f;
            bars.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = pen / 500f;
            bars.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = _shells[idx].Speed / 140f;
            bars.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount = _shells[idx].ShellSO.RicochetAngle / 90f;

            // Value Text
            Transform values = obj.transform.GetChild(3);
            values.GetChild(0).GetComponent<TMP_Text>().text = dmg.ToString();
            values.GetChild(1).GetComponent<TMP_Text>().text = pen.ToString();
            values.GetChild(2).GetComponent<TMP_Text>().text = _shells[idx].Speed.ToString();
            values.GetChild(3).GetComponent<TMP_Text>().text = _shells[idx].ShellSO.RicochetAngle.ToString();

            Toggle toggle = _shellDict[_shells[idx].ID].transform.GetChild(4).GetComponent<Toggle>();
            if (_shellEquipmentData._shellEquipmentList.Contains(_shells[idx].ID))
            {
                toggle.isOn = true;
            }
            else
            {
                toggle.isOn = false;
            }

            obj.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _shellEquipmentData = ShellSaveManager.GetShellEquipment(_currentTankID);
                if (toggle.isOn)
                {
                    int equipIdx = _shellEquipmentData._shellEquipmentList.IndexOf(_shells[idx].ID);
                    _shellEquipDict.Remove(equipIdx);
                    toggle.isOn = false;
                    ShellSaveManager.ShellEquip(_currentTankID, equipIdx, "");
                }
                else
                {
                    if (_shellEquipmentData._shellEquipmentList.Contains("") == false)
                    {
                        WarningShellFull();
                    }
                    else
                    {
                        int equipIdx = _shellEquipmentData._shellEquipmentList.IndexOf("");
                        _shellEquipDict.Add(equipIdx, _shells[idx]);
                        toggle.isOn = true;
                        ShellSaveManager.ShellEquip(_currentTankID, equipIdx, _shells[idx].ID);
                    }
                }
            });
        }
    }

    private void WarningShellFull()
    {
        _warningSequence.Kill();
        _warningSequence = DOTween.Sequence()
        .AppendCallback(() =>
        {
            _warningPanel.GetComponent<CanvasGroup>().DOFade(1, 0f);
            _warningPanel.gameObject.SetActive(true);
            _warningPanel.GetChild(0).GetComponent<TextController>().SetText("Shell is Full!");
        })
        .AppendInterval(1.2f)
        .Append(_warningPanel.GetComponent<CanvasGroup>().DOFade(0, 1f))
        .AppendCallback(() =>
        {
            _warningPanel.gameObject.SetActive(false);
            _warningPanel.GetComponent<CanvasGroup>().DOFade(1, 0f);
        });
    }

    public void PlayButtonSound()
    {
        var audioSource = PoolManager.Get<AudioSourceController>("AudioSource", Vector3.zero, Quaternion.identity);
        audioSource.SetSound(_buttonSound);
        audioSource.SetGroup(AudioMixerType.Sfx);
        audioSource.SetVolume(1f);
        audioSource.Play();
    }
}
