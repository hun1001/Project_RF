using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class EffectAutoPool : AutoPool
{
    protected List<ParticleSystem> _effectList = new List<ParticleSystem>();

    private void Awake() => GetAllParticleSystem();


    private void GetAllParticleSystem()
    {
        _effectList.AddRange(GetComponentsInChildren<ParticleSystem>());
    }

    protected override IEnumerator Pool()
    {
        bool[] isPlaying = new bool[_effectList.Count];

        while (true)
        {
            for (int i = 0; i < _effectList.Count; i++)
            {
                isPlaying[i] = _effectList[i].isPlaying;
            }

            bool isAllStop = true;

            foreach (var isPlay in isPlaying)
            {
                if (isPlay)
                {
                    isAllStop = false;
                    break;
                }
            }

            if (isAllStop)
            {
                break;
            }

            yield return null;
        }


        string id = gameObject.name;

        if (id.Contains("(Clone)"))
        {
            id = id.Replace("(Clone)", "");
        }

        PoolManager.Pool(id, gameObject);
    }
}
