using Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationCanvas : BaseCanvas
{
    [SerializeField]
    private Bar _hpBar = null;
    public Bar HpBar => _hpBar;

    [SerializeField]
    private RectTransform _hitImage = null;

    [SerializeField]
    private Bar _bossHpBar = null;
    public Bar BossHpBar => _bossHpBar;

    private Coroutine _hitCoroutine;

    private Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();

        _hitImage.gameObject.SetActive(false);

        EventManager.StartListening(EventKeyword.PlayerHit, (objects) =>
        {
            if (_hitCoroutine != null) StopCoroutine(_hitCoroutine);
            _hitCoroutine = StartCoroutine(HitEffect(objects));
        });
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
