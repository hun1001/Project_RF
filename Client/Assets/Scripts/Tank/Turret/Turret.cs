using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private TurretSO _turretStatSO = null;
    public TurretSO TurretStatSO => _turretStatSO;

    private TurretSO _thisTurretSO = null;
    public TurretSO TurretData => _thisTurretSO;

    [SerializeField]
    private SoundBoxSO _turretSound = null;
    public SoundBoxSO TurretSound => _turretSound;

    [SerializeField]
    private Transform _turret = null;
    public Transform TurretTransform => _turret;

    [SerializeField]
    private Transform _firePoint = null;
    public Transform FirePoint => _firePoint;

    private Shell _currentShell = null;
    public Shell CurrentShell
    {
        get => _currentShell;
        set => _currentShell = value;
    }

    private void Awake()
    {
        foreach (var component in GetComponents<Turret_Component>())
        {
            _turretComponents.Add(component.ComponentType, component);
        }
        _thisTurretSO = _turretStatSO.Clone();
        CurrentShell = _thisTurretSO.Shells[0];
    }

    private Dictionary<ComponentType, Turret_Component> _turretComponents = new Dictionary<ComponentType, Turret_Component>();

    public T GetComponent<T>(ComponentType componentType) where T : Turret_Component
    {
        if (_turretComponents.ContainsKey(componentType))
        {
            return _turretComponents[componentType] as T;
        }
        Debug.LogError($"Turret doesn't have {componentType} component");
        return null;
    }

    public bool TryGetComponent<T>(ComponentType componentType, out T component) where T : Turret_Component
    {
        if (_turretComponents.ContainsKey(componentType))
        {
            component = _turretComponents[componentType] as T;
            return true;
        }
        component = null;
        return false;
    }

    internal void SetTurretSO(TurretSO turretSO)
    {
        _turretStatSO = turretSO;
    }
}
