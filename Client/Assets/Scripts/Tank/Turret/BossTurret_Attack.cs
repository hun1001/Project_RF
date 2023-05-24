using Pool;
using UnityEngine;

public class BossTurret_Attack : Turret_Attack
{
    private Tank _tank = null;
    private BossTurret _bossTurret = null;

    protected override void Awake()
    {
        _tank = GetComponent<Tank>();
        _bossTurret = Turret as BossTurret;
    }

    public override void Fire()
    {
        if (ReloadingTime <= 0 && gameObject.activeSelf)
        {
            PoolManager.Get<LaserBeam>("LaserBeam", Turret.FirePoint.position, Turret.FirePoint.rotation).SetLaserBeam(_tank, Turret.FirePoint, 500f);
            PoolManager.Get<LaserBeam>("LaserBeam", _bossTurret.FirePoint2.position, _bossTurret.FirePoint2.rotation).SetLaserBeam(_tank, _bossTurret.FirePoint2, 500f);
            ResetReloadTime();
        }
    }

    public void FireMissile(Vector3 _targetPosition)
    {
        if (!(ReloadingTime <= 0 && gameObject.activeSelf))
        {
            return;
        }

        ResetReloadTime();

        Vector3 goalPosition = Vector3.zero;

        for (int i = 0; i < _bossTurret.LeftMissileFirePoints.Length; ++i)
        {
            goalPosition = _targetPosition + Random.insideUnitSphere * 5f;
            goalPosition.z = -0.1f;
            PoolManager.Get<Missile>("Missile", _bossTurret.LeftMissileFirePoints[i].position, _bossTurret.LeftMissileFirePoints[i].rotation).SetMissile(_tank, goalPosition);
        }

        for (int i = 0; i < _bossTurret.RightMissileFirePoints.Length; ++i)
        {
            goalPosition = _targetPosition + Random.insideUnitSphere * 5f;
            goalPosition.z = -0.1f;
            PoolManager.Get<Missile>("Missile", _bossTurret.RightMissileFirePoints[i].position, _bossTurret.RightMissileFirePoints[i].rotation).SetMissile(_tank, goalPosition);
        }
    }
}
