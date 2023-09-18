using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMPBossTurret : Turret
{
    [SerializeField]
    private Transform _firePoint2 = null;
    public Transform FirePoint2 => _firePoint2;

    [SerializeField]
    private Transform[] _leftMissileFirePoints = null;
    public Transform[] LeftMissileFirePoints => _leftMissileFirePoints;

    [SerializeField]
    private Transform[] _rightMissileFirePoints = null;
    public Transform[] RightMissileFirePoints => _rightMissileFirePoints;
}
