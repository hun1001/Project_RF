using UnityEngine;
using Pool;
public class ECdestroyMe : MonoBehaviour, IPoolReset
{

    float timer;
    public float deathtimer = 10;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= deathtimer)
        {
            PoolManager.Pool("MissileExplosionEffect", gameObject);
        }
    }

    public void PoolObjectReset()
    {
        timer = 0;
    }
}
