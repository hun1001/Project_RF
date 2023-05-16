using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Turret_AimLine : Turret_Component
{
    private LineRenderer _lineRenderer = null;
    private Joystick _attackJoystick = null;

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

        //_attackJoystick = FindObjectOfType<ControllerCanvas>().AttackJoystick;

        _lineRenderer.enabled = false;
    }

    private void Update()
    {
        // TODO : 총알 존재 시간 2f -> 총알 속도 * 총알 존재 시간
        _lineRenderer.SetPosition(0, Turret.FirePoint.position);

        var a = Turret.GetComponent<Turret_Attack>(ComponentType.Attack);

        if (a.ReloadingTime <= 0f)
        {
            //_lineRenderer.enabled = true;
            var rayData = Physics2D.Raycast(Turret.FirePoint.position, Turret.FirePoint.up, Turret.CurrentShell.Speed * 2f);

            if (rayData.collider != null && rayData.collider != Turret.GetComponent<Tank>().GetComponent<Collider2D>())
            {
                _lineRenderer.SetPosition(1, rayData.point + (Vector2)Turret.FirePoint.up);
                if (rayData.collider.gameObject.layer == LayerMask.NameToLayer("Tank"))
                {
                    _lineRenderer.colorGradient = _gradients[2];
                }
                else
                {
                    _lineRenderer.colorGradient = _gradients[1];
                }
            }
            else
            {
                _lineRenderer.SetPosition(1, Turret.FirePoint.position + Turret.FirePoint.up * Turret.CurrentShell.Speed * 2f);
                _lineRenderer.colorGradient = _gradients[1];
            }
        }
        else
        {
            var rayData = Physics2D.Raycast(Turret.FirePoint.position, Turret.FirePoint.up, Turret.CurrentShell.Speed * 2f);

            if (rayData.collider != null && rayData.collider != Turret.GetComponent<Tank>().GetComponent<Collider2D>())
            {
                _lineRenderer.SetPosition(1, rayData.point + (Vector2)Turret.FirePoint.up);
            }
            else
            {
                _lineRenderer.SetPosition(1, Turret.FirePoint.position + Turret.FirePoint.up * Turret.CurrentShell.Speed * 2f);
            }

            _lineRenderer.colorGradient = _gradients[0];
            //_lineRenderer.enabled = false;
        }
    }

    public void SetEnableLineLenderer(bool isActive)
    {
        _lineRenderer.enabled = isActive;
    }
}
