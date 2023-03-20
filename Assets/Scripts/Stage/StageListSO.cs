using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace Stage
{
    [CreateAssetMenu(menuName = "SO/Stage/StageListSO")]
    public class StageListSO : ScriptableObject
    {
        /// <summary> 스테이지 리스트 </summary>
        public StageSO[] Stages;

        /// <summary> 맵 리스트 </summary>
        public Map_Information[] Maps;
    }
}
