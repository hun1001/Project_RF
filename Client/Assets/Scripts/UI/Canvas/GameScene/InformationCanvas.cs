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
    private Image _reloadImage = null;
    [SerializeField]
    private RectTransform _hitImage = null;
    private Coroutine _hitCoroutine;

    private Player _player;
    private Turret _playerTurret;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _playerTurret = _player.Tank.Turret;
        _playerTurret.GetComponent<Turret_Attack>().AddOnFireAction(() => StartCoroutine(ReloadCheck()));

        _hitImage.gameObject.SetActive(false);

        EventManager.StartListening(EventKeyword.PlayerHit, (objects) =>
        {
            if (_hitCoroutine != null) StopCoroutine(_hitCoroutine);
            _hitCoroutine = StartCoroutine(HitDirectionCheck(objects));
        });
    }

    private IEnumerator HitDirectionCheck(object[] objects)
    {
        Vector2 dir = new Vector2((float)objects[0], (float)objects[1]);
        dir = Camera.main.WorldToScreenPoint(dir);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _hitImage.gameObject.SetActive(true);
        _hitImage.rotation = Quaternion.AngleAxis(angle - 270, Vector3.forward);
        _hitImage.anchoredPosition = dir.normalized * 20f;

        yield return new WaitForSeconds(1.5f);
        _hitImage.gameObject.SetActive(false);
        _hitImage.anchoredPosition = Vector2.zero;
        _hitCoroutine = null;
    }

    private IEnumerator ReloadCheck()
    {
        float reloadTime = 0;
        float currentTime = 0;

        if (_playerTurret.TurretSO.IsBurst)
        {
            int currentMagazine = _playerTurret.GetComponent<Turret_Attack>().MagazineSize;

            if (currentMagazine > 0)
            {
                reloadTime = _playerTurret.TurretSO.BurstData.BurstReloadTime;
            }
            else
            {
                reloadTime = _playerTurret.TurretSO.ReloadTime;
            }
        }
        else
        {
            reloadTime = _playerTurret.TurretSO.ReloadTime;
        }

        while (currentTime < reloadTime)
        {
            _reloadImage.fillAmount = currentTime / reloadTime;
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
