using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Tank : MonoBehaviour
{
    [SerializeField]
    private TankStatSO _tankStatSO = null;
    public TankStatSO TankStatSO => _tankStatSO;

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
}
