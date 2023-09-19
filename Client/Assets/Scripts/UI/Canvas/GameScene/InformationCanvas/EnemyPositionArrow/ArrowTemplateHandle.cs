using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ArrowTemplateHandle : MonoBehaviour
{
    [SerializeField]
    private Image _arrowImage = null;
    
    private Tank _targetTank = null;
    
    private UnityAction<ArrowTemplateHandle> _onTargetTankDeathAction = null;
    
    private Camera _mainCamera = null;
    private RectTransform _arrowRectTransform => _arrowImage.rectTransform;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    public void SetTargetTank(Tank tank, UnityAction<ArrowTemplateHandle> onTargetTankDeathAction = null)
    {
        _targetTank = tank;
        _onTargetTankDeathAction = onTargetTankDeathAction;
        
        StartCoroutine(ArrowUpdateCoroutine());
    }

    private IEnumerator ArrowUpdateCoroutine()
    {
        while (_targetTank.gameObject.activeSelf)
        {
            Vector3 targetScreenPos = _mainCamera.WorldToScreenPoint(_targetTank.transform.position);

            
            _arrowRectTransform.position = targetScreenPos;

            Vector3 clampedPosition = _arrowRectTransform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, 100, Screen.width - 100);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, 100, Screen.height - 100);
            _arrowRectTransform.position = clampedPosition;

            Vector3 arrowPosition = _arrowRectTransform.position;
            arrowPosition.z = 0;
            _arrowRectTransform.position = arrowPosition;
            
            Vector3 direction = _targetTank.transform.position - _mainCamera.transform.position;
            direction.z = 0;
            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _arrowRectTransform.rotation = Quaternion.Euler(0, 0, angle);
            
            Vector3 viewportPosition = _mainCamera.WorldToViewportPoint(_targetTank.transform.position);
            bool objectInCameraView = viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1 && viewportPosition.z > 0;
            
            _arrowImage.enabled = !objectInCameraView;
            
            yield return null;
        }
        
        OnTargetTankDeath();
    }

    private void OnTargetTankDeath()
    {
        _onTargetTankDeathAction?.Invoke(this);
        gameObject.SetActive(false);
    }
}
