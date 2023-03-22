using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private TurretSO _turretStatSO = null;
    public TurretSO TurretSO => _turretStatSO;

    [SerializeField]
    private SoundBoxSO _turretSound = null;
    public SoundBoxSO TurretSound => _turretSound;

    [SerializeField]
    private Transform _turret = null;
    public Transform TurretTransform => _turret;

    [SerializeField]
    private Transform _firePoint = null;
    public Transform FirePoint => _firePoint;

    private void Awake()
    {
        foreach (var component in GetComponents<Turret_Component>())
        {
            _turretComponents.Add(component.ComponentType, component);
        }
    }

    private Dictionary<ComponentType, Turret_Component> _turretComponents = new Dictionary<ComponentType, Turret_Component>();

    public T GetComponent<T>(ComponentType componentType) where T : Turret_Component
    {
        if (_turretComponents.ContainsKey(componentType))
        {
            return _turretComponents[componentType] as T;
        }
        Debug.LogError($"Tank doesn't have {componentType} component");
        return null;
    }

}
