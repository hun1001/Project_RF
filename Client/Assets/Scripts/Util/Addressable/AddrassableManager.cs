using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using Util;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Addressable
{
    public class AddressablesManager : Singleton<AddressablesManager>
    {
        public T GetResource<T>(string name)
        {
            var handle = Addressables.LoadAssetAsync<T>(name);

            handle.WaitForCompletion();

            return handle.Result;
        }

        public IList<T> GetLabelResources<T>(string label)
        {
            var handle = Addressables.LoadAssetsAsync<T>(label, null);

            handle.WaitForCompletion();

            return handle.Result;
        }

        public void Release<T>(T obj)
        {
            if (obj is GameObject)
            {
                var go = obj as GameObject;
                if (go.scene.IsValid() == false)
                {
                    Addressables.ReleaseInstance(go);
                }
            }
            else
            {
                Addressables.Release(obj);
            }
        }
    }
}