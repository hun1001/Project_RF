using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class RewardManager : MonoSingleton<RewardManager>
{
    private BaseSceneCanvasManager _gameSceneCanvasManager;
    private GameOverCanvas _gameOverCanvas;

    private void Awake()
    {
        _gameSceneCanvasManager = FindObjectOfType<BaseSceneCanvasManager>();
        _gameOverCanvas = _gameSceneCanvasManager.GetComponentInChildren<GameOverCanvas>();
    }

    public void GameOver(bool isClear)
    {
        _gameSceneCanvasManager.ChangeCanvas(CanvasType.GameOver, _gameSceneCanvasManager.ActiveCanvas);
        int rewardValue = 0;
        if (isClear)
        {
            rewardValue = 100;
            GoodsManager.IncreaseFreeGoods(rewardValue);

            _gameOverCanvas.GameResultTextController.SetText("Victory");
        }

        else
        {
            rewardValue = 10;
            GoodsManager.IncreaseFreeGoods(rewardValue);

            _gameOverCanvas.GameResultTextController.SetText("Defeat");
        }

        _gameOverCanvas.RewardValueTextController.SetText(rewardValue);
    }
}
