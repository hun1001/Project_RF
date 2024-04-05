using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using Util;
using UnityEngine;
// using UnityEditor.AddressableAssets.Settings;
// using UnityEditor.AddressableAssets;
// using UnityEditor;


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

        public IList<T> GetLabelResourcesComponents<T>(string label) where T : Component
        {
            var handle = Addressables.LoadAssetsAsync<GameObject>(label, null);
            handle.WaitForCompletion();

            var result = new List<T>();

            foreach (var obj in handle.Result)
            {
                result.Add((obj.GetComponent<T>()));
            }

            return result;
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

        // public void CreateAddressableAsset(string assetPath, string address, string groupName, string label)
        // {
        //     AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

        //     // Check if the group exists, create it if not
        //     AddressableAssetGroup group = settings.FindGroup(groupName);
        //     if (group == null)
        //     {
        //         group = settings.CreateGroup(groupName, false, false, false, null);
        //     }

        //     // Convert asset path to GUID
        //     string assetGUID = AssetDatabase.AssetPathToGUID(assetPath);

        //     // Check if an entry with the same address already exists in the group
        //     AddressableAssetEntry existingEntry = null;

        //     foreach (var entry in group.entries)
        //     {
        //         if (entry.address == address)
        //         {
        //             existingEntry = entry;
        //             break;
        //         }
        //     }

        //     if (existingEntry != null)
        //     {
        //         // If an entry with the same address exists, update its settings
        //         existingEntry.SetAddress(address);
        //         existingEntry.SetLabel(label, true);
        //     }
        //     else
        //     {
        //         // Create a new entry
        //         AddressableAssetEntry newEntry = settings.CreateOrMoveEntry(assetGUID, group);
        //         Debug.Log("Created new entry: " + newEntry);
        //         newEntry.SetAddress(address);
        //         newEntry.SetLabel(label, true);
        //     }

        //     // Mark the settings as dirty and save
        //     settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, group, true);
        //     AssetDatabase.SaveAssets();
        // }

    }
}