using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tank))]
public abstract class Tank_Component : MonoBehaviour
{
    private Tank _tank = null;
    public Tank Tank
    {
        get
        {
            if (_tank is null)
            {
                _tank = GetComponent<Tank>();
            }

            return _tank;
        }
    }

    [SerializeField]
    private ComponentType _componentType = ComponentType.None;

    public ComponentType ComponentType => _componentType;
}
