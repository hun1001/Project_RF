using Event;
using UnityEngine;
using Util;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    // Tutorial ing
    private bool _isTutorial = false;
    public bool IsTutorial => _isTutorial;

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
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.TutorialScene)
        {
            _isTutorial = true;
            WaveManager.Instance.gameObject.SetActive(false);
            FindObjectOfType<BaseSceneCanvasManager>().ChangeCanvas(CanvasType.GameTutorial);
        }
    }

    public void TankDummySpawn()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.TutorialScene)
        {
            var tank = SpawnManager.Instance.SpawnUnit("BT-5", new Vector3(22f, 25f, 0f), Quaternion.identity, GroupType.Enemy);
            tank.GetComponent<Tank_Damage>().AddOnDeathAction(() => EventManager.TriggerEvent(EventKeyword.NextTutorial));
        }
    }

    public void MovingTargetSpawn()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.TutorialScene)
        {
            GameObject.Find("MoveTarget").transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void TutorialWaveStart()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.TutorialScene)
        {
            WaveManager.Instance.gameObject.SetActive(true);
        }
    }
}
