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
        _angle = 80f;
        _range = 40f;
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

        Collider2D[] cols;
        Vector2 machingunPosition;
        Transform enemy;
        float nearDist;
        Transform target;
        Vector2 dirToTarget;
        float dist;
        Vector2 directionV2;
        Vector3 directionV3;
        Quaternion directionRotation;

        while (true)
        {
            yield return shotDelay;

            enemy = null;
            nearDist = Mathf.Infinity;
            machingunPosition = transform.position;

            cols = Physics2D.OverlapCircleAll(machingunPosition, _range, _layerMask);
            for (int i = 0; i < cols.Length; i++)
            {
                // 플레이어 제외
                if (cols[i].CompareTag("Player")) continue;
                
                target = cols[i].transform;
                dirToTarget = (target.position - transform.position);

                // FOV
                // 머신건과 적 사이에 플레이어가 있으면 플레이어가 맞기에 시야각을 설정함
                if (Vector2.Angle(transform.forward, dirToTarget.normalized) < _angle / 2)
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

            directionV2 = (enemy.position - transform.position).normalized;
            directionV3 = new Vector3(-directionV2.x, 0, directionV2.y);
            directionRotation = Quaternion.LookRotation(directionV3);
            directionRotation = Quaternion.Euler(0, 0, directionRotation.eulerAngles.y);

            // 총알 생성, 총알 데미지 설정
            PoolManager.Get("MachingunShell", transform.position, directionRotation);

            _currentMagazine--;
            if (_currentMagazine <= 0)
            {
                _currentMagazine = _maxMagazine;
                yield return reloadTime;
            }
        }
    }
}
