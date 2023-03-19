using System;
using UnityEngine;

namespace Util
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly Lazy<T> _instance =
            new Lazy<T>(() =>
            {
                T instance = FindObjectOfType(typeof(T)) as T;

                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).FullName);
                    instance = obj.AddComponent(typeof(T)) as T;

                    DontDestroyOnLoad(obj);
                }
                DontDestroyOnLoad(instance);

                return instance;
            });

        public static T Instance
        {
            get
            {
                return _instance.Value;
            }
        }
    }

}