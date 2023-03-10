using Util;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private void Awake()
        {
            Application.targetFrameRate = 120;
            PlayerPrefs.SetInt("Gold", 100);
            PlayerPrefs.SetInt("Destroy", 0);
        }
    }
}
