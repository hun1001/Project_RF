using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapSprite : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer = null;
    private Transform _indicator = null;
    private MinimapCameraManager _minimapCam;

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
                _minimapCam.ShowBorderIndicator(transform, _indicator);
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
