using Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class RewardManager : MonoBehaviour
{
    private void Awake()
    {
        EventManager.StartListening(EventKeyword.BossClear, () => GameOver(true));
        EventManager.StartListening(EventKeyword.PlayerDead, () => GameOver(false));
    }

    private void GameOver(bool isClear)
    {
        int rewardValue = 0;
        if (isClear)
        {
            rewardValue = 100;
            GoodsManager.IncreaseFreeGoods(rewardValue);
        }

        else
        {
            rewardValue = 10;
            GoodsManager.IncreaseFreeGoods(rewardValue);
        }
    }
}
