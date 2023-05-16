using System.Collections;
using UnityEngine;
using Pool;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    private Tank _tank = null;
    private NavMeshPath _navMeshPath = null;
    private BehaviorTree _behaviorTree = null;

    private Tank_Move _tankMove = null;
    private Tank_Rotate _tankRotate = null;

    private Turret_Rotate _turretRotate = null;
    private Turret_Attack _turretAttack = null;

    private void Awake()
    {
        _tank = PoolManager.Get("BMP-130-2").GetComponent<Tank>();
        _navMeshPath = new NavMeshPath();

        _tankMove = _tank.GetComponent<Tank_Move>(ComponentType.Move);
        _tankRotate = _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate);

        _turretRotate = _tank.Turret.GetComponent<Turret_Rotate>(ComponentType.Rotate);
        _turretAttack = _tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack);
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void Attack()
    {
        _turretAttack.Fire();
    }

    private void Move(Vector3 position)
    {
        if (NavMesh.CalculatePath(_tank.transform.position, position, NavMesh.AllAreas, _navMeshPath))
        {
            StartCoroutine(MoveTarget(0, _navMeshPath.corners.Length));
        }
    }

    private IEnumerator MoveTarget(int index, int pathLength)
    {
        if (index < pathLength)
        {
            while (Vector3.Distance(_tank.transform.position, _navMeshPath.corners[index]) > 1f)
            {
                _tankMove.Move(0.9f);
                _tankRotate.Rotate((_navMeshPath.corners[index] - _tank.transform.position).normalized);
                yield return null;
            }

            StartCoroutine(MoveTarget(index + 1, pathLength));
        }
    }
}
