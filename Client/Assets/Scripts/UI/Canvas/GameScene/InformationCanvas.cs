using Event;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private GameObject _bossPanel = null;
    [SerializeField]
    private TextController _bossName = null;
    [SerializeField]
    private Bar _bossHpBar = null;
    public Bar BossHpBar => _bossHpBar;

    [Header("Stage")]
    [SerializeField]
    private GameObject _stagePanel = null;
    [SerializeField]
    private TextController _enemyCnt = null;

    private Coroutine _hitCoroutine;
    private Player _player;

    [Header("Shell")]
    [SerializeField]
    private GameObject _shellImageGroup = null;
    public GameObject ShellImageGroup => _shellImageGroup;

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

    private void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.GameScene)
        {
            _stagePanel.SetActive(false);
            _bossPanel.SetActive(false);
        }

        //else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.StageScene)
        //{
        //    _bossPanel.SetActive(false);
        //    _stagePanel.SetActive(true);
        //    StageEnemyCntUpdate();
        //    EventManager.StartListening(EventKeyword.EnemyDie, StageEnemyCntUpdate);
        //}

        else // Tranining
        {
            _bossPanel.SetActive(false);
            _stagePanel.SetActive(false);
        }
    }

    public void BossInfoSetting()
    {
        //¸¸µé¾î¾ßµÊ
    }

    //private void Update()
    //{
    //    if (_controllerCanvas.AttackJoystick.DragTime > 0f && _tankMove.CurrentSpeed == 0)
    //    {
    //        if (_controllerCanvas.AttackJoystick.DragTime <= 3f)
    //        {
    //            if (_isDirty == false)
    //            {
    //                _isDirty = true;
    //            }

    //            _chargingIndex = (int)_controllerCanvas.AttackJoystick.DragTime;

    //            _chargingImages[_chargingIndex].fillAmount = (_controllerCanvas.AttackJoystick.DragTime - _chargingIndex);

    //            if (_chargingIndex > 0 && _chargingIndex < 3 && _chargingImages[_chargingIndex - 1].fillAmount < 1f)
    //            {
    //                _chargingImages[_chargingIndex - 1].fillAmount = 1f;
    //            }
    //        }
    //        else if (_chargingImages[2].fillAmount < 1f)
    //        {
    //            _chargingImages[2].fillAmount = 1f;
    //        }
    //    }
    //    else if (_isDirty)
    //    {
    //        for (int i = 0; i < 3; i++)
    //        {
    //            _chargingImages[i].fillAmount = 0f;
    //        }
    //        _isDirty = false;
    //    }
    //}

    private void StageEnemyCntUpdate()
    {
        if (GameWay_Base.RemainingEnemy > 0)
        {
            _enemyCnt.SetText(GameWay_Base.RemainingEnemy);
        }
        else
        {
            _enemyCnt.SetText("Clear");
        }
    }

    private IEnumerator HitEffect(object[] objects)
    {
        Vector2 dir = new Vector2((float)objects[0], (float)objects[1]);
        dir = Camera.main.WorldToScreenPoint(dir);
        Vector2 playerPos = Camera.main.WorldToScreenPoint(_player.Tank.transform.position);
        float angle = Mathf.Atan2(dir.y - playerPos.y, dir.x - playerPos.x) * Mathf.Rad2Deg;
        _hitImage.gameObject.SetActive(true);
        _hitImage.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        _hitImage.anchoredPosition = (dir - playerPos).normalized * 20f;

        yield return new WaitForSeconds(1.5f);
        _hitImage.gameObject.SetActive(false);
        _hitCoroutine = null;
    }
}
