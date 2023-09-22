using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KVBossTurret : Turret
{
    [SerializeField]
    private Transform _secondTurret = null;
    public Transform SecondTurret => _secondTurret;

    [SerializeField]
    private Transform _thirdTurret = null;
    public Transform ThirdTurret => _thirdTurret;

    [SerializeField]
    private Transform _thirdFirePoint = null;
    public Transform ThirdFirePoint => _thirdFirePoint;
}
