using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCanvas : BaseCanvas
{
    [SerializeField]
    private TextController _gameModeTextController;
    public TextController GameModeTextController => _gameModeTextController;

    [SerializeField]
    private TextController _gameResultTextController;
    public TextController GameResultTextController => _gameResultTextController;

    [SerializeField]
    private TextController _rewardValueTextController;
    public TextController RewardValueTextController => _rewardValueTextController;

    public override void OnOpenEvents()
    {
        Time.timeScale = 0f;
        base.OnOpenEvents();
    }
}
