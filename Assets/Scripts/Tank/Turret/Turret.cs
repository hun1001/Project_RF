using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : CustomObject
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
}
