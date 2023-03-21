using Addressable;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Pool
{
    public class PoolManager : Singleton<PoolManager>
    {
        private Dictionary<string, Queue<GameObject>> _poolingDictionaryQueue = new Dictionary<string, Queue<GameObject>>();

        public GameObject Get(string name)
        {
            return GetObject(name);
        }

        public GameObject Get(string name, Transform parent)
        {
            var temp = GetObject(name);
            temp.transform.SetParent(parent);

            return temp;
        }

        public GameObject Get(string name, Vector3 position, Quaternion rotation)
        {
            var temp = GetObject(name);
            temp.transform.position = position;
            temp.transform.rotation = rotation;

            return temp;
        }

        public GameObject Get(GameObject obj)
        {
            return GetObject(obj);
        }

        public GameObject Get(GameObject obj, Transform position)
        {
            GameObject temp = GetObject(obj);
            temp.transform.SetParent(position);

            return temp;
        }

        public GameObject Get(GameObject obj, Vector3 position, Quaternion rotation)
        {
            GameObject temp = GetObject(obj);

            temp.transform.position = position;
            temp.transform.rotation = rotation;

            return temp;
        }

        public T Get<T>(string name) where T : MonoBehaviour => GetObject(name).GetComponent<T>();

        public T Get<T>(string name, Transform parent) where T : MonoBehaviour => Get(name, parent).GetComponent<T>();

        public T Get<T>(string name, Vector3 position, Quaternion rotation) where T : MonoBehaviour => Get(name, position, rotation).GetComponent<T>();

        public T Get<T>(GameObject obj) where T : MonoBehaviour => GetObject(obj).GetComponent<T>();

        public T Get<T>(GameObject obj, Transform parent) where T : MonoBehaviour => Get(obj, parent).GetComponent<T>();

        public T Get<T>(GameObject obj, Vector3 position, Quaternion rotation) where T : MonoBehaviour => Get(obj, position, rotation).GetComponent<T>();

        public void Pool(string name, GameObject obj)
        {
            obj.SetActive(false);

            if (_poolingDictionaryQueue.ContainsKey(name))
            {
                _poolingDictionaryQueue[name].Enqueue(obj);
            }
            else
            {
                _poolingDictionaryQueue.Add(name, new Queue<GameObject>());
                _poolingDictionaryQueue[name].Enqueue(obj);
            }
        }

        public void Pool(GameObject obj) => Pool(obj.name, obj);

        private GameObject GetObject(string name)
        {
            GameObject temp = null;

            if (_poolingDictionaryQueue.ContainsKey(name))
            {
                if (_poolingDictionaryQueue[name].Count > 0)
                {
                    temp = _poolingDictionaryQueue[name].Dequeue();
                }
                else
                {
                    temp = GameObject.Instantiate(AddressablesManager.Instance.GetResource<GameObject>(name), null);
                }
            }
            else
            {
                _poolingDictionaryQueue.Add(name, new Queue<GameObject>());
                temp = GameObject.Instantiate(AddressablesManager.Instance.GetResource<GameObject>(name), null);
            }

            temp.SetActive(true);

            return temp;
        }

        private GameObject GetObject(GameObject obj)
        {
            GameObject temp = null;

            if (_poolingDictionaryQueue.ContainsKey(obj.name))
            {
                if (_poolingDictionaryQueue[obj.name].Count > 0)
                {
                    temp = _poolingDictionaryQueue[obj.name].Dequeue();
                }
                else
                {
                    temp = GameObject.Instantiate(obj, null);
                }
            }
            else
            {
                _poolingDictionaryQueue.Add(obj.name, new Queue<GameObject>());
                temp = GameObject.Instantiate(obj, null);
            }

            temp.SetActive(true);

            return temp;
        }

        public void Clear()
        {
            _poolingDictionaryQueue.Clear();
        }
    }
}