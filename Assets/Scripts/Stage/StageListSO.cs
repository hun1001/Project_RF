using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
    [CreateAssetMenu(menuName = "SO/Stage/StageListSO")]
    public class StageListSO : ScriptableObject
    {
        public StageSO[] Stages;
        public GameObject[] Maps;
    }
}
