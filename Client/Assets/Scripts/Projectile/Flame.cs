using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    List<Tank_Damage> _damagedTanks = new List<Tank_Damage>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        Tank_Damage td = null;
        if (other.TryGetComponent<Tank_Damage>(out td))
        {
            if (!_damagedTanks.Contains(td))
            {
                _damagedTanks.Add(td);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Tank_Damage td = null;
        if (other.TryGetComponent<Tank_Damage>(out td))
        {
            if (_damagedTanks.Contains(td))
            {
                _damagedTanks.Remove(td);
            }
        }
    }

    private void LateUpdate()
    {
        if (_damagedTanks.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < _damagedTanks.Count; i++)
        {
            _damagedTanks[i].Damaged(1, 99999, transform.position, Vector3.zero);
        }
    }
}
