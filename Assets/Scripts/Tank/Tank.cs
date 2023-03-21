using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

[DisallowMultipleComponent]
public class Tank : MonoBehaviour
{
    [SerializeField]
    private TankSO _tankStatSO = null;
    public TankSO TankStatSO => _tankStatSO;

    [SerializeField]
    private Joystick _joystick = null;

    [SerializeField]
    private Joystick _turretJoystick = null;

    private Turret _turret = null;

    private Dictionary<ComponentType, Tank_Component> _tankComponents = new Dictionary<ComponentType, Tank_Component>();

    private void Awake()
    {
        _turret = GetComponent<Turret>();
        foreach (var tankComponent in GetComponents<Tank_Component>())
        {
            _tankComponents.Add(tankComponent.ComponentType, tankComponent);
        }
    }

    public T GetComponent<T>(ComponentType componentType) where T : Tank_Component
    {
        if (_tankComponents.ContainsKey(componentType))
        {
            return _tankComponents[componentType] as T;
        }
        Debug.LogError($"Tank doesn't have {componentType} component");
        return null;
    }

    private void Update()
    {
        if (_joystick != null)
        {
            GetComponent<Tank_Move>(ComponentType.Move).Move(_joystick.Magnitude);
            GetComponent<Tank_Rotate>(ComponentType.Rotate).Rotate(_joystick.Direction);
        }
    }
}
