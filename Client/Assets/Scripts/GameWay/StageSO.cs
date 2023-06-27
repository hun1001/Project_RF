using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
    [CreateAssetMenu(menuName = "SO/Stage/StageSO")]
    public class StageSO : ScriptableObject
    {
        /// <summary> 적 리스트 </summary>
        public GameObject[] Enemys;
    }
}
