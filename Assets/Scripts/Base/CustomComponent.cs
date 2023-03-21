using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomComponent : MonoBehaviour
{
    private CustomObject _instance = null;
    public CustomObject Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = GetComponent<CustomObject>();
            }

            return _instance;
        }
    }

    [SerializeField]
    private ComponentType _componentType = ComponentType.None;

    public ComponentType ComponentType => _componentType;
}
