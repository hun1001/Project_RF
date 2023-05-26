using Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationCanvas : BaseCanvas
{
    [Header("Player")]
    [SerializeField]
    private Bar _hpBar = null;
    public Bar HpBar => _hpBar;

    [SerializeField]
    private RectTransform _hitImage = null;

    [Header("Boss")]
    [SerializeField]
    private Bar _bossHpBar = null;
    public Bar BossHpBar => _bossHpBar;

    private Coroutine _hitCoroutine;
    private Player _player;

    [Header("Charging")]
    [SerializeField]
    private ControllerCanvas _controllerCanvas;
    [SerializeField]
    private Image[] _chargingImages;
    private Tank_Move _tankMove;
    private bool _isDirty = false;

    private int _chargingIndex;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _tankMove = _player.Tank.GetComponent<Tank_Move>();

        _hitImage.gameObject.SetActive(false);

        EventManager.StartListening(EventKeyword.PlayerHit, (objects) =>
        {
            if (_hitCoroutine != null) StopCoroutine(_hitCoroutine);
            _hitCoroutine = StartCoroutine(HitEffect(objects));
        });
    }

    private void Update()
    {
        if (_controllerCanvas.AttackJoystick.DragTime > 0f && _tankMove.CurrentSpeed == 0)
        {
            if (_controllerCanvas.AttackJoystick.DragTime <= 3f)
            {
                if (_isDirty == false)
                {
                    _isDirty = true;
                }

                _chargingIndex = (int)_controllerCanvas.AttackJoystick.DragTime;

                _chargingImages[_chargingIndex].fillAmount = (_controllerCanvas.AttackJoystick.DragTime - _chargingIndex);

                if (_chargingIndex > 0 && _chargingIndex < 3 && _chargingImages[_chargingIndex - 1].fillAmount < 1f)
                {
                    _chargingImages[_chargingIndex - 1].fillAmount = 1f;
                }
            }
            else if (_chargingImages[2].fillAmount < 1f)
            {
                _chargingImages[2].fillAmount = 1f;
            }
        }
        else if (_isDirty)
        {
            for (int i = 0; i < 3; i++)
            {
                _chargingImages[i].fillAmount = 0f;
            }
            _isDirty = false;
        }
    }

    private IEnumerator HitEffect(object[] objects)
    {
        Vector2 dir = new Vector2((float)objects[0], (float)objects[1]);
        dir = Camera.main.WorldToScreenPoint(dir);
        Vector2 playerPos = Camera.main.WorldToScreenPoint(_player.Tank.transform.position);
        float angle = Mathf.Atan2(dir.y - playerPos.y, dir.x - playerPos.x) * Mathf.Rad2Deg;
        _hitImage.gameObject.SetActive(true);
        _hitImage.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y - playerPos.y, dir.x - playerPos.x) * Mathf.Rad2Deg + 90f);
        _hitImage.anchoredPosition = (dir - playerPos).normalized * 20f;

        yield return new WaitForSeconds(1.5f);
        _hitImage.gameObject.SetActive(false);
        _hitCoroutine = null;
    }
}
