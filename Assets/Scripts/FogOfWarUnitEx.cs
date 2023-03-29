using FoW;
using UnityEngine;

public class FogOfWarUnitEx : FogOfWarUnit
{
    private void Update()
    {
        if (FindObjectOfType<FogOfWarTeam>().GetFogValue(transform.position) > 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
