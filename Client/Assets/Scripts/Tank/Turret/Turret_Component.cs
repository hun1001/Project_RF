using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Turret))]
public abstract class Turret_Component : MonoBehaviour
{
    private Turret _turret = null;
    public Turret Turret
    {
        get
        {
            if (_turret is null)
            {
                _turret = GetComponent<Turret>();
            }

            return _turret;
        }
    }

    [SerializeField]
    private ComponentType _componentType = ComponentType.None;

    public ComponentType ComponentType => _componentType;
}
