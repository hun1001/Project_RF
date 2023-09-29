using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using Util;
using UnityEngine;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor;

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

        public void AddressSetting(string path, string address, string groupName, string label)
        {
            Debug.LogWarning("This function has Errors.");
            return;

            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            AddressableAssetGroup group = settings.FindGroup(groupName);
            if (group == null)
            {
                group = settings.CreateGroup(groupName, false, false, false, null);
            }

            AddressableAssetEntry entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path), group);

            entry.SetAddress(address);
            entry.SetLabel(label, true);

            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

            AssetDatabase.SaveAssets();
        }
    }
}