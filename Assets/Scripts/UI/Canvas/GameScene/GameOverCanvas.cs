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

        EventManager.StartListening(EventKeyword.StageClear, () => StartCoroutine(StageModeGameOver(true)));
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.GameScene)
        {
            EventManager.StartListening(EventKeyword.PlayerDead, () => StartCoroutine(BossModeGameOver(false)));
        }
        else
        {
            EventManager.StartListening(EventKeyword.PlayerDead, () => StartCoroutine(StageModeGameOver(false)));
        }
    }

    private void Start()
    {
        AddInputAction();
    }

    protected override void AddInputAction()
    {
        KeyboardManager.Instance.AddKeyDownAction(KeyCode.Return, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType)
            {
                OnHomeButton();
            }
        });
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
            _gameResultTextController.SetText("<color=#FFEA00>" + "Victory" + "</color>");
            _rewardValueTextController.SetText(rewardValue);
        }
        else
        {
            rewardValue = Random.Range(5, 15);
            _gameResultTextController.SetText("<color=#C20000>" + "Defeat" + "</color>");
            _rewardValueTextController.SetText(rewardValue);
        }

        EventManager.TriggerEvent(EventKeyword.GiveReward, rewardValue);
    }

    private IEnumerator StageModeGameOver(bool isClear)
    {
        int rewardValue = 0;
        yield return new WaitForSeconds(2f);

        _gameModeTextController.SetText(string.Format("Stage " + (GameWay_Base.CurrentStage + 1).ToString()));
        CanvasManager.ChangeCanvas(CanvasType);

        rewardValue = GameWay_Base.Instance.StageListSO.Stages[GameWay_Base.CurrentStage].Reward;
        
        if (isClear)
        {
            _gameResultTextController.SetText("<color=#FFEA00>Clear</color>");
            _rewardValueTextController.SetText(rewardValue);
        }
        else
        {
            int min = GameWay_Base.Instance.StageListSO.Stages[GameWay_Base.CurrentStage].MinDefeatPercent;
            int max = GameWay_Base.Instance.StageListSO.Stages[GameWay_Base.CurrentStage].MaxDefeatPercent;
            int percent = Random.Range(min, max);
            
            rewardValue = (int)(rewardValue * 0.01f * percent);
            _gameResultTextController.SetText("<color=#C20000>Defeat</color>");
            _rewardValueTextController.SetText(rewardValue);
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.TutorialScene)
        {
            _rewardValueTextController.SetText(50);
            yield break;
        }
        EventManager.TriggerEvent(EventKeyword.GiveReward, rewardValue);
    }
}
