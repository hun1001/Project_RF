using Event;
using System.Net.NetworkInformation;
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

    private GameObject _waveManager = null;

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
        _waveManager = FindObjectOfType<WaveManager>().gameObject;
        _waveManager.SetActive(false);
    }

    public void TankDummySpawn(string tankID, Vector3 pos)
    {
        var tank = SpawnManager.Instance.SpawnUnit(tankID, pos, Quaternion.identity, GroupType.Enemy);
        tank.GetComponent<Tank_Damage>().AddOnDeathAction(() => EventManager.TriggerEvent(EventKeyword.NextTutorial));
    }

    public void MovingTargetSpawn()
    {
        GameObject.Find("MoveTarget").transform.GetChild(0).gameObject.SetActive(true);
    }

    public void TutorialWaveStart()
    {
        _waveManager.SetActive(true);
    }
}
