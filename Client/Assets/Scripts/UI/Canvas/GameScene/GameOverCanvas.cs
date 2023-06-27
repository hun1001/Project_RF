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
        // 게임 모드에 따라서 달라져야함
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
        int rewardValue = 0;
        yield return new WaitForSeconds(2f);

        _gameModeTextController.SetText("BOSS");
        CanvasManager.ChangeCanvas(CanvasType);

        // 보스에 따라서 보상 값 달라져야함
        if (isClear)
        {
            rewardValue = 100;
            _gameResultTextController.SetText("Victory");
            _rewardValueTextController.SetText(rewardValue);
        }
        else
        {
            rewardValue = Random.Range(5, 15);
            _gameResultTextController.SetText("Defeat");
            _rewardValueTextController.SetText(rewardValue);
        }

        EventManager.TriggerEvent(EventKeyword.GiveReward, rewardValue);
    }

    private IEnumerator StageModeGameOver(bool isClear)
    {
        yield return new WaitForSeconds(2f);

    }
}
