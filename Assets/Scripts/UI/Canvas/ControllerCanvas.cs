using System.Collections;
using System.Collections.Generic;
using Custom;
using UnityEngine;

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

}
