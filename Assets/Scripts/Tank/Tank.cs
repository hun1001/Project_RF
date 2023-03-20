using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Tank : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField]
    private TankStatSO _tankStatSO = null;

    private Turret _turret = null;
}
