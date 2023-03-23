using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Tank : CustomObject
{
    [SerializeField]
    private TankSO _tankSO = null;
    public TankSO TankSO => _tankSO;

    [SerializeField]
    private SoundBoxSO _tankSound = null;
    public SoundBoxSO TankSound => _tankSound;

    private GroupType _groupType = GroupType.None;

    private Turret _turret = null;
    public Turret Turret => _turret;


    protected override void Awake()
    {
        base.Awake();
        _turret = GetComponent<Turret>();
    }
}
