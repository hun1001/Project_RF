using Pool;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SubBattery : BaseSubArmament
{
    [SerializeField]
    private LineRenderer _lineRenderer = null;

    public override SubArmamentKeyActionType ActionType => SubArmamentKeyActionType.OnKeyDownUp;

    public override void Aim()
    {
        _lineRenderer.enabled = true;

        _lineRenderer.SetPosition(0, FirePoint.position);
        _lineRenderer.SetPosition(1, FirePoint.position + Vector3.up * 50f);
    }

    public override void Fire()
    {
        _lineRenderer.enabled = false;

        PoolManager.Get<Shell>("Bullet", FirePoint.position, FirePoint.rotation).SetShell(GetComponent<Tank>(), 200, 40);
        PoolManager.Get("MuzzleFlash4", FirePoint.position, FirePoint.rotation);
    }
}
