using System.Collections;
using UnityEngine;
using Pool;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    private Tank _tank = null;
    private NavMeshPath _navMeshPath = null;

    private void Start()
    {
        _tank = PoolManager.Get("BMP-130-2").GetComponent<Tank>();
        _navMeshPath = new NavMeshPath();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo))
            {
                Move(hitInfo.point);
            }
        }

        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo))
            {
                Vector2 direction = (_tank.Turret.FirePoint.position - hitInfo.point).normalized;
                _tank.Turret.GetComponent<Turret_Rotate>(ComponentType.Rotate).Rotate((_tank.Turret.FirePoint.position - hitInfo.point).normalized);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _tank.Turret.GetComponent<Turret_Attack>(ComponentType.Attack).Fire();
        }
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
                _tank.GetComponent<Tank_Move>(ComponentType.Move).Move(0.9f);
                _tank.GetComponent<Tank_Rotate>(ComponentType.Rotate).Rotate((_navMeshPath.corners[index] - _tank.transform.position).normalized);
                yield return null;
            }

            StartCoroutine(MoveTarget(index + 1, pathLength));
        }
    }
}
