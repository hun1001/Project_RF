using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Event;
using Addressable;
using Pool;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ShellReplacement : MonoBehaviour, IButtonSound
{
    [Header("Sound")]
    [SerializeField]
    private AudioClip _buttonSound = null;

    [Header("Shell")]
    [SerializeField]
    private GameObject _template = null;
    [SerializeField]
    private RectTransform _parent = null;
    [SerializeField]
    private GameObject _shellInformation = null;

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
            EventTrigger.Entry pointDown = new EventTrigger.Entry();

            if (_shellDict.ContainsKey(_shells[idx].ID))
            {
                _shellDict[_shells[idx].ID].SetActive(true);

                if (_shellEquipmentData._shellEquipmentList.Contains(_shells[i].ID))
                {
                    _shellDict[_shells[idx].ID].transform.GetChild(0).GetComponent<Toggle>().isOn = true;
                    _shellEquipDict.Add(_shellEquipmentData._shellEquipmentList.IndexOf(_shells[idx].ID), _shells[idx]);
                }
                else
                {
                    _shellDict[_shells[i].ID].transform.GetChild(0).GetComponent<Toggle>().isOn = false;
                }

                _shellDict[_shells[idx].ID].transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
                _shellDict[_shells[idx].ID].transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() =>
                {
                    PlayButtonSound();
                    _shellInformation.SetActive(false);
                    _shellEquipmentData = ShellSaveManager.GetShellEquipment(_currentTankID);

                    if (_shellDict[_shells[idx].ID].transform.GetChild(0).GetComponent<Toggle>().isOn)
                    {
                        int equipIdx = _shellEquipmentData._shellEquipmentList.IndexOf(_shells[idx].ID);
                        _shellEquipDict.Remove(equipIdx);
                        _shellDict[_shells[idx].ID].transform.GetChild(0).GetComponent<Toggle>().isOn = false;
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
                            _shellDict[_shells[idx].ID].transform.GetChild(0).GetComponent<Toggle>().isOn = true;
                            ShellSaveManager.ShellEquip(_currentTankID, equipIdx, _shells[idx].ID);
                        }
                    }
                });

                _shellDict[_shells[idx].ID].transform.GetChild(3).GetComponent<EventTrigger>().triggers.Clear();

                pointDown.eventID = EventTriggerType.PointerDown;
                pointDown.callback.AddListener((data) =>
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        float dmg = Mathf.Round(_shells[idx].ShellSO.Damage * (Mathf.Pow(turret.TurretSO.AtkPower, 2) * 0.001f));
                        float pen = Mathf.Round(turret.TurretSO.AtkPower * turret.TurretSO.PenetrationPower * _shells[idx].ShellSO.Penetration / 3000f);

                        _shellInformation.SetActive(true);
                        _shellInformation.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = _shells[idx].ShellSprite;
                        _shellInformation.transform.GetChild(0).GetChild(1).GetComponent<TextController>().SetText(_shells[idx].ID);

                        // Bar
                        Transform bars = _shellInformation.transform.GetChild(2);
                        bars.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = dmg / 1500f;
                        bars.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = pen / 500f;
                        bars.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = _shells[idx].Speed / 140f;
                        bars.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount = _shells[idx].ShellSO.RicochetAngle / 90f;

                        // Value Text
                        Transform values = _shellInformation.transform.GetChild(3);
                        values.GetChild(0).GetComponent<TextController>().SetText(dmg.ToString());
                        values.GetChild(1).GetComponent<TextController>().SetText(pen.ToString());
                        values.GetChild(2).GetComponent<TextController>().SetText(_shells[idx].Speed.ToString());
                        values.GetChild(3).GetComponent<TextController>().SetText(_shells[idx].ShellSO.RicochetAngle.ToString());
                    }
                });
                _shellDict[_shells[idx].ID].transform.GetChild(3).GetComponent<EventTrigger>().triggers.Add(pointDown);

                continue;
            }

            var obj = Instantiate(_template, _parent);
            _shellDict.Add(_shells[idx].ID, obj);
            obj.SetActive(true);
            Sprite shellSprite = _shells[idx].ShellSprite;
            string shellID = _shells[idx].ID;

            obj.transform.GetChild(1).GetComponent<Image>().sprite = shellSprite;
            obj.transform.GetChild(2).GetComponent<TextController>().SetText(shellID);

            Toggle toggle = _shellDict[_shells[idx].ID].transform.GetChild(0).GetComponent<Toggle>();
            if (_shellEquipmentData._shellEquipmentList.Contains(_shells[idx].ID))
            {
                toggle.isOn = true;
            }
            else
            {
                toggle.isOn = false;
            }

            obj.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _shellInformation.SetActive(false);
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

            pointDown.eventID = EventTriggerType.PointerDown;
            pointDown.callback.AddListener((data) =>
            {
                if (Input.GetMouseButtonDown(1))
                {
                    float dmg = Mathf.Round(_shells[idx].ShellSO.Damage * (Mathf.Pow(turret.TurretSO.AtkPower, 2) * 0.001f));
                    float pen = Mathf.Round(turret.TurretSO.AtkPower * turret.TurretSO.PenetrationPower * _shells[idx].ShellSO.Penetration / 3000f);

                    _shellInformation.SetActive(true);
                    _shellInformation.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = _shells[idx].ShellSprite;
                    _shellInformation.transform.GetChild(0).GetChild(1).GetComponent<TextController>().SetText(_shells[idx].ID);

                    // Bar
                    Transform bars = _shellInformation.transform.GetChild(2);
                    bars.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = dmg / 1500f;
                    bars.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = pen / 500f;
                    bars.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = _shells[idx].Speed / 140f;
                    bars.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount = _shells[idx].ShellSO.RicochetAngle / 90f;

                    // Value Text
                    Transform values = _shellInformation.transform.GetChild(3);
                    values.GetChild(0).GetComponent<TextController>().SetText(dmg.ToString());
                    values.GetChild(1).GetComponent<TextController>().SetText(pen.ToString());
                    values.GetChild(2).GetComponent<TextController>().SetText(_shells[idx].Speed.ToString());
                    values.GetChild(3).GetComponent<TextController>().SetText(_shells[idx].ShellSO.RicochetAngle.ToString());
                }
            });
            obj.transform.GetChild(3).GetComponent<EventTrigger>().triggers.Add(pointDown);
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

    public void PlayButtonSound(AudioClip audioClip = null)
    {
        var audioSource = PoolManager.Get<AudioSourceController>("AudioSource", Vector3.zero, Quaternion.identity);
        if (audioClip == null) audioClip = _buttonSound;
        audioSource.SetSound(_buttonSound);
        audioSource.SetGroup(AudioMixerType.Sfx);
        audioSource.SetVolume(1f);
        audioSource.Play();
    }
}
