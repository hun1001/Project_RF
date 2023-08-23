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
    private ToggleGroupManager shellToggleManager = null;
    public ToggleGroupManager ShellToggleManager => shellToggleManager;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

}
