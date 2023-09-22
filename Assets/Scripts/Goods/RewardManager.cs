using Event;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    private void Awake()
    {
        EventManager.StartListening(EventKeyword.GiveReward, GiveReward);
    }

    private void GiveReward(object[] objects)
    {
        int rewardValue = (int)objects[0];

        GoodsManager.IncreaseFreeGoods(rewardValue);
    }
}
