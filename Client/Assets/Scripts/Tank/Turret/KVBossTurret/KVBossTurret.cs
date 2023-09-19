using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KVBossTurret : Turret
{
    [SerializeField]
    private Transform _secondTurret = null;

    [SerializeField]
    private Transform _thirdTurret = null;

    [SerializeField]
    private Transform _thirdFirePoint = null;
    public Transform ThirdFirePoint => _thirdFirePoint;
}
