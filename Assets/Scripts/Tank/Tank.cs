using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

[DisallowMultipleComponent]
public class Tank : MonoBehaviour
{
    [SerializeField]
    private TankSO _tankSO = null;
    public TankSO TankSO => _tankSO;

    private Turret _turret = null;
    public Turret Turret => _turret;

    private void Awake()
    {
        _turret = GetComponent<Turret>();
        foreach (var component in GetComponents<Tank_Component>())
        {
            _tankComponents.Add(component.ComponentType, component);
        }
    }

    private Dictionary<ComponentType, Tank_Component> _tankComponents = new Dictionary<ComponentType, Tank_Component>();

    public T GetComponent<T>(ComponentType componentType) where T : Tank_Component
    {
        if (_tankComponents.ContainsKey(componentType))
        {
            return _tankComponents[componentType] as T;
        }
        Debug.LogError($"Tank doesn't have {componentType} component");
        return null;
    }
}
