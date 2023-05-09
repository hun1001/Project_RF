using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TankTechTreeNode : MonoBehaviour
{
    [SerializeField]
    private Image _tankTypeIcon = null;

    [SerializeField]
    private Text _tankTierText = null;

    [SerializeField]
    private Text _tankNameText = null;

    [SerializeField]
    private Image _tankLockImage = null;

    [SerializeField]
    private Button _button = null;

    private bool _isTankLocked = false;
    public bool IsTankLocked
    {
        get => _isTankLocked;
        set
        {
            _isTankLocked = value;
            _tankLockImage.enabled = _isTankLocked;
        }
    }

    public void SetTankNode(Sprite tankTypeIcon, string tankTier, string tankName, bool isTankLocked, UnityAction onClick)
    {
        _tankTypeIcon.sprite = tankTypeIcon;
        _tankTierText.text = tankTier;
        _tankNameText.text = tankName;
        _isTankLocked = isTankLocked;

        _tankLockImage.enabled = _isTankLocked;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            onClick();
        });
    }
}
