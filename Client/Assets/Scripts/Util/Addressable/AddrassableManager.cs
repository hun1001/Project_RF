using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using Util;

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
    }
}