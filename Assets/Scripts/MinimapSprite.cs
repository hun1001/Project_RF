using Pool;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MinimapSprite : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer = null;
    /// <summary> 해당 오브젝트를 가르키는 화살표 </summary>
    private Transform _indicator = null;
    /// <summary> 미니맵 카메라 매니저 </summary>
    private MinimapCameraManager _minimapCam;

    /// <summary> 부모 </summary>
    private Transform _parent = null;

    private void Awake()
    {
        TryGetComponent(out _spriteRenderer);
        _parent = transform.parent;
    }

    private void Start()
    {
        _minimapCam = FindObjectOfType<MinimapCameraManager>();

        if (_parent.CompareTag("Player"))
        {
            _spriteRenderer.color = Color.green;
        }
        else
        {
            _spriteRenderer.color = Color.red;
            StartCoroutine(CheckVisible());
        }
    }

    /// <summary> 이 스프라이트가 미니맵에 보이는지 체크하는 코루틴 </summary>
    private IEnumerator CheckVisible()
    {
        while (true)
        {
            if (_spriteRenderer.isVisible == false)
            {
                if (_indicator == null)
                {
                    _indicator = PoolManager.Get("Indicator", _minimapCam.transform).transform;
                }
                _minimapCam.IndicatorPositionSetting(transform, _indicator);
            }
            else if (_indicator != null)
            {
                PoolManager.Pool("Indicator", _indicator.gameObject);
                _indicator = null;
            }

            yield return null;
        }
    }
}
