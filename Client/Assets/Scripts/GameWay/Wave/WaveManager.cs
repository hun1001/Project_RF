using Event;
using Pool;
using System.Collections;
using UnityEngine;

public class WaveManager : GameWay_Base
{
    private void Start()
    {
        Spawn();
    }

    protected override void Spawn()
    {
        if (StageListSO.Stages[CurrentStage].IsBoss)
        {
            string bossAIAddress = (StageListSO.Stages[CurrentStage].Enemys[0] as BossTank).AI_Address;

            var boss = PoolManager.Get<BossAI_Base>(bossAIAddress, _currentMap.RandomSpawnPoint(), Quaternion.identity);
            boss.Init("");

            var bar = FindObjectOfType<BossBar>();
            bar.Setting(boss.Tank.TankData.HP);

            boss.TankDamage.AddOnDamageAction(bar.ChangeValue);
            boss.TankDamage.AddOnDeathAction(bar.Hide);

            RemainingEnemy = 1;
        }
        else
        {
            base.Spawn();
        }
        EventManager.TriggerEvent("Next");
    }

    public override void StageClear()
    {
        if(CurrentStage + 1 >= StageListSO.Stages.Length)
        {
            EventManager.TriggerEvent(EventKeyword.StageClear);
            return;
        }

        CurrentStage++;
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(3f);
        Spawn();
    }
}
