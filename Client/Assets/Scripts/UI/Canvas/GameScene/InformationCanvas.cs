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

    private Player _player;
    private Turret _playerTurret;

    private void Awake()
    {
        _reloadImage.gameObject.SetActive(false);
        _player = FindObjectOfType<Player>();
        Tank pt = _player.Tank;
        _playerTurret = pt.Turret;
        _playerTurret.GetComponent<Turret_Attack>().AddOnFireAction(() => StartCoroutine(ReloadCheck())); 
    }

    private IEnumerator ReloadCheck()
    {
        float reloadTime = _playerTurret.TurretSO.ReloadTime;
        float currentTime = 0;
        _reloadImage.gameObject.SetActive(true);

        while (currentTime < reloadTime)
        {
            _reloadImage.fillAmount = currentTime / reloadTime;
            currentTime += Time.deltaTime;
            yield return null;
        }

        _reloadImage.gameObject.SetActive(false);
    }
}
