using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
    [CreateAssetMenu(menuName = "SO/Stage/StageSO")]
    public class StageSO : ScriptableObject
    {
        // 적 목록들
        public GameObject[] Enemys;
    }
}
