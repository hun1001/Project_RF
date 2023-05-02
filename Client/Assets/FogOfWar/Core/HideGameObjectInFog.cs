using UnityEngine;

namespace FoW
{
    [AddComponentMenu("FogOfWar/HideGameObjectInFog")]
    public class HideGameObjectInFog : MonoBehaviour
    {
        public int team = 0;

        [Range(0.0f, 1.0f)]
        public float minFogStrength = 0.2f;

        Transform _transform;
        Renderer[] _renderer;

        void Start()
        {
            _transform = transform;
            _renderer = GetComponentsInChildren<Renderer>();
        }

        void Update()
        {
            FogOfWarTeam fow = FogOfWarTeam.GetTeam(team);
            if (fow == null)
            {
                Debug.LogWarning("There is no Fog Of War team for team #" + team.ToString());
                return;
            }

            bool visible = fow.GetFogValue(_transform.position) < minFogStrength * 255;
            if (_renderer.Length > 0)
            {
                foreach (Renderer renderer in _renderer)
                {
                    renderer.enabled = visible;
                }
            }
        }
    }
}