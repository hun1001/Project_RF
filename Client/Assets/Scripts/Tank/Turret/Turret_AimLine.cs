using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Turret_AimLine : Turret_Component
{
    private LineRenderer _lineRenderer = null;

    private bool _isAim = false;
    public bool IsAim => _isAim;

    private Gradient[] _gradients = new Gradient[3];

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _gradients[0] = new Gradient()
        {
            colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(Color.red, 0f),
                new GradientColorKey(Color.red, 1f)
            },
            alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        };

        _gradients[1] = new Gradient()
        {
            colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(Color.yellow, 0f),
                new GradientColorKey(Color.yellow, 1f)
            },
            alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        };

        _gradients[2] = new Gradient()
        {
            colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(Color.green, 0f),
                new GradientColorKey(Color.green, 1f)
            },
            alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        };

        _lineRenderer.enabled = false;
    }

    private void Update()
    {
        // TODO : 총알 존재 ?�간 2f -> 총알 ?�도 * 총알 존재 ?�간
        _lineRenderer.SetPosition(0, Turret.FirePoint.position);

        var a = Turret.GetComponent<Turret_Attack>(ComponentType.Attack);

        var rayData = Physics2D.Raycast(Turret.FirePoint.position, Turret.FirePoint.up, Turret.CurrentShell.Speed * 2f, 1 << LayerMask.NameToLayer("Tank") | 1 << LayerMask.NameToLayer("Wall"));
        if (a.ReloadingTime <= 0f)
        {
            if (rayData.collider != null && rayData.collider != Turret.GetComponent<Tank>().GetComponent<Collider2D>())
            {
                _isAim = rayData.collider.gameObject.layer == LayerMask.NameToLayer("Tank");
                _lineRenderer.SetPosition(1, new Vector3(rayData.point.x, rayData.point.y, Turret.FirePoint.position.z));
                _lineRenderer.colorGradient = IsAim ? _gradients[2] : _gradients[1];
            }
            else
            {
                _isAim = false;
                _lineRenderer.SetPosition(1, Turret.FirePoint.position + Turret.FirePoint.up * Turret.CurrentShell.Speed * 2f);
                _lineRenderer.colorGradient = _gradients[1];
            }
        }
        else
        {
            _isAim = rayData.collider != null && rayData.collider != Turret.GetComponent<Tank>().GetComponent<Collider2D>();

            var pos = _isAim ? (Vector3)(rayData.point + (Vector2)Turret.FirePoint.up) : Turret.FirePoint.position + Turret.FirePoint.up * Turret.CurrentShell.Speed * 2f;
            pos.z = Turret.FirePoint.position.z;

            _lineRenderer.SetPosition(1, pos);

            _lineRenderer.colorGradient = _gradients[0];
        }
    }

    public void SetEnableLineRenderer(bool isActive)
    {
        _lineRenderer.enabled = isActive;
    }
}
