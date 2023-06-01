using Event;
using System.Collections;
using UnityEngine;

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
        EventManager.StartListening(EventKeyword.BossClear, () => StartCoroutine(BossModeGameOver(true)));
        EventManager.StartListening(EventKeyword.PlayerDead, () => StartCoroutine(BossModeGameOver(false)));
    }

    public override void OnOpenEvents()
    {
        Time.timeScale = 0f;
        base.OnOpenEvents();
    }

    private IEnumerator BossModeGameOver(bool isClear)
    {
        yield return new WaitForSeconds(2f);

        _gameModeTextController.SetText("BOSS");
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
