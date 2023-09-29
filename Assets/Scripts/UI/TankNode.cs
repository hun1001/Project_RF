using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class TankNode : MonoBehaviour
{
    [SerializeField]
    private Image _tankTypeIcon = null;

    [SerializeField]
    private TextMeshProUGUI _tankTierText = null;

    [SerializeField]
    private TextMeshProUGUI _tankNameText = null;

    [SerializeField]
    private Button _button = null;

    public void SetTankNode(Sprite tankTypeIcon, string tankTier, string tankName, UnityAction onClick)
    {
        _tankTypeIcon.sprite = tankTypeIcon;
        _tankTierText.text = tankTier;
        _tankNameText.text = tankName;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            onClick();
        });
    }

    public void SetActive(bool active) => gameObject.SetActive(active);
}
