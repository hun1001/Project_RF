using System.Collections.Generic;
using UnityEngine;
using System;
using Addressable;

namespace Pool
{
    public static class PoolManager
    {
        private static Dictionary<string, Queue<GameObject>> _pool = new Dictionary<string, Queue<GameObject>>();
        private static Dictionary<string, GameObject> _prefabDictionary = new Dictionary<string, GameObject>();
        private static List<string> _nameList = new List<string>();

        private static void CreatePool(string name)
        {
            if (_pool.ContainsKey(name))
            {
                return;
            }

            Queue<GameObject> q = new Queue<GameObject>();
            GameObject prefab = AddressablesManager.Instance.GetResource<GameObject>(name);

            try
            {
                _pool.Add(name, q);
                _nameList.Add(name);
                _prefabDictionary.Add(name, prefab.gameObject);
            }
            catch (ArgumentException e)
            {
                Debug.Log(e.ToString());

                _pool.Clear();
                _prefabDictionary.Clear();
                _pool.Add(name, q);
                _nameList.Add(name);
                _prefabDictionary.Add(name, prefab.gameObject);
            }
        }

        public static GameObject Load(string name)
        {
            if (!_prefabDictionary.ContainsKey(name))
            {
                CreatePool(name);
            }

            return _prefabDictionary[name];
        }

        public static void Pool(string name, GameObject obj)
        {
            if (!_pool.ContainsKey(name))
            {
                CreatePool(name);
            }

            if (obj.TryGetComponent<IPoolReset>(out var c))
            {
                c.PoolObjectReset();
            }

            obj.SetActive(false);
            obj.transform.SetParent(null);
            _pool[name].Enqueue(obj);
        }

        public static void DeleteAllPool()
        {
            for (int i = 0; i < _nameList.Count; ++i)
            {
                var q = _pool[_nameList[i]];
                while (q.Count > 0)
                {
                    var obj = q.Dequeue();
                    GameObject.Destroy(obj);
                    AddressablesManager.Instance.Release(obj);
                }
            }

            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        public static GameObject Get(string name, bool active = true)
        {
            GameObject item = null;

            if (!_prefabDictionary.ContainsKey(name))
            {
                CreatePool(name);
            }

            if (_pool.ContainsKey(name))
            {
                Queue<GameObject> q = _pool[name];

                if (q.Count == 0)
                {
                    GameObject prefab = _prefabDictionary[name];
                    GameObject g = GameObject.Instantiate(prefab);
                    item = g;
                }
                else
                {
                    item = q.Dequeue();
                }
            }
            else
            {
                GameObject prefab = _prefabDictionary[name];
                GameObject g = GameObject.Instantiate(prefab);
                item = g;
            }

            item.gameObject.SetActive(active);
            return item;
        }

        public static GameObject Get(string name, Transform parent, bool active = true)
        {
            GameObject item = Get(name, active);

            item.transform.SetParent(parent);

            return item;
        }

        public static GameObject Get(string name, Vector3 position, Quaternion rotation, bool active = true)
        {
            GameObject item = Get(name, active);

            item.transform.position = position;
            item.transform.rotation = rotation;

            return item;
        }

        public static GameObject Get(string name, Vector3 position, Quaternion rotation, Transform parent, bool active = true)
        {
            GameObject item = Get(name, position, rotation, active);

            item.transform.SetParent(parent);

            return item;
        }

        public static T Get<T>(string name, bool active = true) where T : Component => Get(name, active).GetComponent<T>();

        public static T Get<T>(string name, Transform parent, bool active = true) where T : Component => Get(name, parent, active).GetComponent<T>();

        public static T Get<T>(string name, Vector3 position, Quaternion rotation, bool active = true) where T : Component => Get(name, position, rotation, active).GetComponent<T>();

        public static T Get<T>(string name, Vector3 position, Quaternion rotation, Transform parent, bool active = true) where T : Component => Get(name, position, rotation, parent, active).GetComponent<T>();

        public static T Load<T>(string name) where T : Component => Load(name).GetComponent<T>();
    }
}