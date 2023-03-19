using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace Stage
{
    [CreateAssetMenu(menuName = "SO/Stage/StageListSO")]
    public class StageListSO : ScriptableObject
    {
        public StageSO[] Stages;
        public Map_Information[] Maps;
    }
}
