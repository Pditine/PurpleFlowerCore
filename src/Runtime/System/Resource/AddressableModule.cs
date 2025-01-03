using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PurpleFlowerCore.Resource
{
    public class AddressableModule
    {
        private readonly Dictionary<string, IEnumerator> _resDic = new();
        
        public void Load<T>(string name, Action<AsyncOperationHandle<T>> callBack)
        {
            string keyName = GetKeyName(name, typeof(T));
            AsyncOperationHandle<T> handle;
            if (_resDic.ContainsKey(keyName))
            {
                handle = (AsyncOperationHandle<T>)_resDic[keyName];
                if (handle.IsDone)
                {
                    callBack(handle);
                }
                else
                {
                    handle.Completed += operation =>
                    {
                        if (operation.Status == AsyncOperationStatus.Succeeded)
                        {
                            callBack(handle);
                        }
                    };
                }
                return;
            }

            handle = Addressables.LoadAssetAsync<T>(name);
            handle.Completed += operation =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    callBack(handle);
                }
                else
                {
                    PFCLog.Warning("Addressable", $"LoadAssetAsync Failed: {name}");
                    if(_resDic.ContainsKey(keyName))
                        _resDic.Remove(keyName);
                }
            };
            _resDic.Add(keyName, handle);
        }
        
        public void Release<T>(string name)
        {
            string keyName = GetKeyName(name, typeof(T));
            if (_resDic.ContainsKey(keyName))
            {
                AsyncOperationHandle<T> handle = (AsyncOperationHandle<T>)_resDic[keyName];
                Addressables.Release(handle);
                _resDic.Remove(keyName);
            }
        }

        // public void Clear()
        // {
        //     _resDic.Clear();
        //     AssetBundle.UnloadAllAssetBundles(true);
        //     Resources.UnloadUnusedAssets();
        //     GC.Collect();
        // }
        
        private string GetKeyName(string name, Type type)
        {
            return name + "_" + type.Name;
        }
    }
}