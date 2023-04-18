using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TankNode : MonoBehaviour
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
    private EventTrigger _eventTrigger = null;

    private bool _isTankLocked = false;

    public void SetTankNode(Sprite tankTypeIcon, string tankTier, string tankName, bool isTankLocked, UnityAction<BaseEventData> onClick)
    {
        _tankTypeIcon.sprite = tankTypeIcon;
        _tankTierText.text = tankTier;
        _tankNameText.text = tankName;
        _isTankLocked = isTankLocked;

        _tankLockImage.enabled = _isTankLocked;

        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;

        entry.callback.AddListener(onClick);

        _eventTrigger.triggers.Add(entry);
    }
}
