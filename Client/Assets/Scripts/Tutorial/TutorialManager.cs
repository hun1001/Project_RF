using Event;
using UnityEngine;
using Util;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    // Tutorial ing
    private bool _isTutorial = false;
    public bool IsTutorial => _isTutorial;

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
        _waveManager = FindObjectOfType<WaveManager>().gameObject;
        _waveManager.SetActive(false);
    }

    public void TankDummySpawn()
    {
        var tank = SpawnManager.Instance.SpawnUnit("BT-5", new Vector3(22f, 25f, 0f), Quaternion.identity, GroupType.Enemy);
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
