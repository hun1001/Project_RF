using UnityEngine;
using FoW;

[DisallowMultipleComponent]
public class Tank : CustomObject, IPoolReset
{
    [SerializeField]
    private TankSO _tankSO = null;
    public TankSO TankSO => _tankSO;

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

    private MinimapSprite _minimapSprite = null;

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
        _minimapSprite = GetComponentInChildren<MinimapSprite>();
    }

    public void PoolObjectReset()
    {
        _thisTankSO = _tankSO.Clone();
        GetComponent<Tank_Damage>(ComponentType.Damage).ResetData();
        _minimapSprite.Disabled();
    }

    internal void SetTankSO(TankSO tankSO)
    {
        _tankSO = tankSO;
    }
}
