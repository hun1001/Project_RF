using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item_Machingun : Item.Item_Base
{
    [SerializeField]
    protected float[] _upgradeDamages;
    protected float _currentDamage;

    protected int _maxMagazine = 50;
    protected int _currentMagazine = 0;

    private float _angle = 80f;
    private float _range = 40f;
    private int _layerMask = 0;

    protected override void CreateItem()
    {
        SetPosAndRot();

        _currentDamage = _upgradeDamages[0];
        _currentMagazine = _maxMagazine;
        _layerMask = 1 << LayerMask.NameToLayer("Tank");
        StartCoroutine(Shot());
    }

    /// <summary> 머신건의 로컬 좌표와 방향을 설정하는 함수 </summary>
    protected abstract void SetPosAndRot();

    protected override void UpgradeItem()
    {
        _currentDamage = _upgradeDamages[Item.ItemManager.Instance.HaveItemList[this]];
    }

    /// <summary> 머신건 발사 시작 코루틴 </summary>
    private IEnumerator Shot()
    {
        WaitForSeconds reloadTime = new WaitForSeconds(4f);
        WaitForSeconds shotDelay = new WaitForSeconds(0.2f);

        Collider[] cols;
        Transform enemy;
        float nearDist;
        Transform target;
        Vector3 dirToTarget;
        float dist;
        Quaternion dir;

        while (true)
        {
            yield return shotDelay;

            enemy = null;
            nearDist = Mathf.Infinity;

            cols = Physics.OverlapSphere(transform.position, _range, _layerMask);
            for (int i = 0; i < cols.Length; i++)
            {
                // 플레이어 제외
                if (cols[i].CompareTag("Player")) continue;

                target = cols[i].transform;
                dirToTarget = (target.position - transform.position);

                // FOV
                // 머신건과 적 사이에 플레이어가 있으면 플레이어가 맞기에 시야각을 설정함
                if (Vector3.Angle(transform.forward, dirToTarget.normalized) < _angle / 2)
                {
                    // 가장 가까운 적을 때리기 위해
                    dist = dirToTarget.sqrMagnitude;
                    if (nearDist > dist)
                    {
                        nearDist = dist;
                        enemy = target;
                    }
                }
            }
            // 적을 찾지 못하면 재탐색
            if (enemy == null) continue;

            dir = Quaternion.LookRotation(enemy.position - transform.position);
            dir.x = 0f;
            dir.z = 0f;

            // 총알 생성, 총알 데미지 설정

            _currentMagazine--;
            if (_currentMagazine <= 0)
            {
                _currentMagazine = _maxMagazine;
                yield return reloadTime;
            }
        }
    }
}
