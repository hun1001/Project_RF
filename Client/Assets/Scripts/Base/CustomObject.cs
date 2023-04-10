using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class CustomObject : MonoBehaviour
{
    [SerializeField]
    private string _id = null;
    public string ID => _id;

    private Dictionary<ComponentType, CustomComponent> _components = new Dictionary<ComponentType, CustomComponent>();

    protected virtual void Awake()
    {
        foreach (var component in GetComponents<CustomComponent>())
        {
            _components.Add(component.ComponentType, component);
        }
    }

    public T GetComponent<T>(ComponentType componentType) where T : CustomComponent
    {
        if (_components.ContainsKey(componentType))
        {
            return _components[componentType] as T;
        }
        Debug.LogError($"Tank doesn't have {componentType} component");
        return null;
    }

    public bool TryGetComponent<T>(ComponentType componentType, out T component) where T : CustomComponent
    {
        if (_components.ContainsKey(componentType))
        {
            component = _components[componentType] as T;
            return true;
        }
        component = null;
        return false;
    }
}
