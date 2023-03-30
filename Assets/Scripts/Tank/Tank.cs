using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoW;

[DisallowMultipleComponent]
public class Tank : CustomObject, IPoolReset
{
    [SerializeField]
    private TankSO _tankSO = null;

    private TankSO _thisTankSO = null;
    public TankSO TankData => _thisTankSO;

    [SerializeField]
    private SoundBoxSO _tankSound = null;
    public SoundBoxSO TankSound => _tankSound;

    private GroupType _groupType = GroupType.None;
    public GroupType GroupType => _groupType;

    private Turret _turret = null;
    public Turret Turret => _turret;

    private FogOfWarUnit _fogOfWarUnit = null;

    public Tank SetTank(GroupType groupType)
    {
        _groupType = groupType;
        _fogOfWarUnit.team = (int)groupType;
        return this;
    }

    protected override void Awake()
    {
        base.Awake();
        _turret = GetComponent<Turret>();
        _fogOfWarUnit = GetComponent<FogOfWarUnit>();
        _thisTankSO = _tankSO.Clone();
    }

    public void PoolObjectReset()
    {
        _thisTankSO = _tankSO.Clone();
    }
}
