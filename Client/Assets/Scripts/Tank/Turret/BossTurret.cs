using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurret : Turret
{
    [SerializeField]
    private Transform _firePoint2 = null;
    public Transform FirePoint2
    {
        get => _firePoint2;
        set => _firePoint2 = value;
    }
}
