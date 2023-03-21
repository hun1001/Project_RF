using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomObject : MonoBehaviour
{
    // private Dictionary<ComponentType, T> _components = new Dictionary<ComponentType, T>();

    // private void Awake()
    // {
    //     foreach (var component in GetComponents<T>())
    //     {
    //         _components.Add(component.ComponentType, component);
    //     }
    // }

    // public U GetComponent<U>(ComponentType componentType) where U : CustomComponent<T>
    // {
    //     if (_components.ContainsKey(componentType))
    //     {
    //         return _components[componentType] as U;
    //     }
    //     Debug.LogError($"Tank doesn't have {componentType} component");
    //     return null;
    // }
}
