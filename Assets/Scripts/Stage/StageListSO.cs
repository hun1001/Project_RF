using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace Stage
{
    [CreateAssetMenu(menuName = "SO/Stage/StageListSO")]
    public class StageListSO : ScriptableObject
    {
        // 몇 개의 스테이지로 구성되어 있는가
        public StageSO[] Stages;

        // 몇 개의 맵이 등장하는가
        public Map_Information[] Maps;
    }
}
