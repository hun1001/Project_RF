using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Tank : MonoBehaviour
{
    [SerializeField]
    private TankSO _tankSO = null;
    public TankSO TankSO => _tankSO;

    [SerializeField]
    private SoundBoxSO _tankSound = null;
    public SoundBoxSO TankSound => _tankSound;

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

    public bool TryGetComponent<T>(ComponentType componentType, out T component) where T : Tank_Component
    {
        if (_tankComponents.ContainsKey(componentType))
        {
            component = _tankComponents[componentType] as T;
            return true;
        }
        component = null;
        return false;
    }
}
