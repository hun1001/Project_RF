using Event;
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

    private void Awake()
    {
        EventManager.StartListening(EventKeyword.BossClear, () => BossModeGameOver(true));
        EventManager.StartListening(EventKeyword.PlayerDead, () => BossModeGameOver(false));
    }

    public override void OnOpenEvents()
    {
        Time.timeScale = 0f;
        base.OnOpenEvents();
    }

    private void BossModeGameOver(bool isClear)
    {
        CanvasManager.ChangeCanvas(CanvasType);
        if (isClear)
        {
            _gameResultTextController.SetText("Victory");
            _rewardValueTextController.SetText(100);
        }
        else
        {
            _gameResultTextController.SetText("Defeat");
            _rewardValueTextController.SetText(10);
        }
    }
}
