using UnityEngine;

[DisallowMultipleComponent]
public class Tank : CustomObject, IPoolReset
{
    [SerializeField]
    private TankSO _tankSO = null;
    public TankSO TankSO
    {
        get => _tankSO;
        set => _tankSO = value;
    }

    private TankSO _thisTankSO = null;
    public TankSO TankData => _thisTankSO;

    [SerializeField]
    private SoundBoxSO _tankSound = null;
    public SoundBoxSO TankSound => _tankSound;

    private GroupType _groupType = GroupType.None;
    public GroupType GroupType => _groupType;

    private Turret _turret = null;
    public Turret Turret => _turret;

    public bool IsDead => _thisTankSO.HP <= 0;

    private SubArmament _secondaryArmament = null;
    public bool TryGetSecondaryArmament(out SubArmament secondaryArmament)
    {
        secondaryArmament = _secondaryArmament;
        return _hasSecondaryArmament;
    }

    private bool _hasSecondaryArmament = false;

    public Tank SetTank(GroupType groupType, SubArmament secondaryArmament = null)
    {
        _hasSecondaryArmament = secondaryArmament != null;
        _secondaryArmament = secondaryArmament?.Setting(this, Turret.SecondFirePoint);

        _groupType = groupType;
        return this;
    }

    protected override void Awake()
    {
        base.Awake();
        _turret = GetComponent<Turret>();
        _thisTankSO = _tankSO.Clone();
    }

    public void PoolObjectReset()
    {
        _thisTankSO = _tankSO.Clone();
        GetComponent<Tank_Damage>(ComponentType.Damage).ResetData();
    }
}
