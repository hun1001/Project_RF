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
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (NavMesh.CalculatePath(_tank.transform.position, hitInfo.point, NavMesh.AllAreas, _navMeshPath))
                {
                    StartCoroutine(MoveTarget(0, _navMeshPath.corners.Length));
                    for (int i = 0; i < _navMeshPath.corners.Length - 1; i++)
                    {
                        Debug.DrawLine(_navMeshPath.corners[i], _navMeshPath.corners[i + 1], Color.red, 10f);
                    }
                }
            }
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
