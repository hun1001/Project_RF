using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Turret_AimLine : Turret_Component
{
    private LineRenderer _lineRenderer = null;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        // TODO : 총알 존재 시간 2f -> 총알 속도 * 총알 존재 시간

        _lineRenderer.SetPosition(0, Turret.FirePoint.position);

        var rayData = Physics2D.Raycast(Turret.FirePoint.position, Turret.FirePoint.up, Turret.CurrentShell.Speed * 2f);
        if (rayData.collider != null)
        {
            _lineRenderer.SetPosition(1, rayData.point + (Vector2)Turret.FirePoint.up);
        }
        else
        {
            _lineRenderer.SetPosition(1, Turret.FirePoint.position + Turret.FirePoint.up * Turret.CurrentShell.Speed * 2f);
        }

        _lineRenderer.renderingLayerMask = Turret.GetComponent<Turret_Attack>().IsReload ? 0u : 1u;
    }
}
