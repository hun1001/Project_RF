using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
    [CreateAssetMenu(menuName = "SO/Stage/StageSO")]
    public class StageSO : ScriptableObject
    {
        /// <summary> 적 리스트 </summary>
        public Tank[] Enemys;

        public int Reward = 0;
        [Range(0, 50)]
        public int MinDefeatPercent = 0;
        [Range(0, 50)]
        public int MaxDefeatPercent = 50;
    }
}
