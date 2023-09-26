using Event;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    // Tutorial ing
    private bool _isTutorial = false;
    public bool IsTutorial => _isTutorial;

    private bool _isCanMove = false;
    public bool IsCanMove
    {
        get
        {
            return _isCanMove;
        }
        set
        {
            _isCanMove = value;
        }
    }

    private bool _isCanAttack = false;
    public bool IsCanAttack
    {
        get
        {
            return _isCanAttack;
        }
        set
        {
            _isCanAttack = value;
        }
    }

    private bool _isCanChangeShell = false;
    public bool IsCanChangeShell
    {
        get
        {
            return _isCanChangeShell;
        }
        set
        {
            _isCanChangeShell = value;
        }
    }

    private bool _isCanSAT = false;
    public bool IsCanSAT
    {
        get
        {
            return _isCanSAT;
        }
        set
        {
            _isCanSAT = value;
        }
    }

    private GameObject _waveManager = null;
    private Tank _tankDummy = null;
    private List<Tank> _dummyList = new List<Tank>();
    private Vector3 _dir = Vector3.zero;

    public void TutorialStart()
    {
        _isTutorial = true;
    }

    public void TutorialSkip()
    {
        _isTutorial = false;
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    public void GameTutorialStart()
    {
        _isTutorial = true;
        _isCanMove = false;
        _isCanAttack = false;
        _isCanChangeShell = false;
        _isCanSAT = false;

        _waveManager = FindObjectOfType<WaveManager>().gameObject;
        _waveManager.SetActive(false);
        
        _dummyList.Clear();
        _dummyList.Add(SpawnManager.Instance.SpawnUnit("Tiger I", new Vector3(64f, 82f, 0f), Quaternion.Euler(0f, 0f, Quaternion.LookRotation(new Vector3(64f, 0f, -82f).normalized).eulerAngles.y), GroupType.Enemy));
        _dummyList.Add(SpawnManager.Instance.SpawnUnit("Maus", new Vector3(80f, 85f, 0f), Quaternion.Euler(0f, 0f, Quaternion.LookRotation(new Vector3(80f, 0f, -85f).normalized).eulerAngles.y), GroupType.Enemy));
        _dummyList.Add(SpawnManager.Instance.SpawnUnit("Leopard 1", new Vector3(84f, 70f, 0f), Quaternion.Euler(0f, 0f, Quaternion.LookRotation(new Vector3(84f, 0f, -70f).normalized).eulerAngles.y), GroupType.Enemy));

    }

    public void DummyRemove()
    {
        PoolManager.Pool("Tiger I", _dummyList[0].gameObject);
        PoolManager.Pool("Maus", _dummyList[1].gameObject);
        PoolManager.Pool("Leopard 1", _dummyList[2].gameObject);

        _dummyList.Clear();
    }

    public void TankDummySpawn(string tankID, Vector3 spawnPos)
    {
        _tankDummy = SpawnManager.Instance.SpawnUnit(tankID, spawnPos, Quaternion.identity, GroupType.Enemy);
        _tankDummy.GetComponent<Tank_Damage>().AddOnDeathAction(() => EventManager.TriggerEvent(EventKeyword.NextTutorial));

        EnemyBar enemyBar = _tankDummy.GetComponentInChildren<EnemyBar>();

        if (enemyBar != null)
        {
            enemyBar.PoolObjectReset();
        }
        else
        {
            enemyBar = PoolManager.Get<EnemyBar>("EnemyBar", _tankDummy.transform);
        }

        enemyBar.Setting(_tankDummy.TankData.HP);

        _tankDummy.GetComponent<Tank_Damage>().AddOnDamageAction(enemyBar.ChangeValue);
        _tankDummy.GetComponent<Tank_Damage>().AddOnDamageAction((_) => enemyBar.Show());
    }

    public void TankDummyMove(Vector3 movePos)
    {
        StartCoroutine(DummyMoveCoroutine(movePos));
    }

    private IEnumerator DummyMoveCoroutine(Vector3 movePos)
    {
        _dir = movePos - _tankDummy.transform.position;
        VirtualCameraManager.Instance.SetTargetCamera(_tankDummy.gameObject);
        VirtualCameraManager.Instance.SwitchingCamera();

        while (_dir.magnitude >= 2)
        {
            _dir = movePos - _tankDummy.transform.position;

            _tankDummy.GetComponent<Tank_Move>().Move(1f);
            _tankDummy.GetComponent<Tank_Rotate>().Rotate(_dir.normalized);

            yield return null;
        }

        _tankDummy.GetComponent<Tank_Move>().Move(0f);
        VirtualCameraManager.Instance.SwitchingCamera(false);
    }

    public void MovingTargetSpawn(int idx)
    {
        GameObject.Find("MoveTarget").transform.GetChild(idx).gameObject.SetActive(true);
    }

    public void TutorialWaveStart()
    {
        _waveManager.SetActive(true);
        _isCanSAT = true;
        _isCanChangeShell = true;
        _isCanAttack = true;
        _isCanMove = true;
    }
}
