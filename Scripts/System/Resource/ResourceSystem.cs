using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace PurpleFlowerCore
{
    public static class ResourceSystem
    {
        #region Resources

//#if PFC_RES_RESOURCES
        public static Object LoadResource(string path)
        {
            return Resources.Load(path);
        }
        
        public static Object LoadResource(string path,Type type)
        {
            return Resources.Load(path,type);
        }
        
        public static T LoadResource<T>(string path) where T: Object
        {
            return Resources.Load<T>(path);
        }

        public static void LoadResourceAsync(string path,UnityAction<Object> callBack)
        {
            MonoSystem.Start_Coroutine(DoLoadResourceAsync(path, callBack));
        }

        private static IEnumerator DoLoadResourceAsync(string path,UnityAction<Object> callBack)
        {
            var res = Resources.LoadAsync(path);
            yield return res;
            callBack?.Invoke(res.asset);
        }

        public static void LoadResourceAsync(string path,Type type,UnityAction<Object> callBack)
        {
            MonoSystem.Start_Coroutine(DoLoadResourceAsync(path,type,callBack));
        }

        private static IEnumerator DoLoadResourceAsync(string path,Type type,UnityAction<Object> callBack)
        {
            var res = Resources.LoadAsync(path,type);
            yield return res;
            callBack?.Invoke(res.asset);
        }

        public static void LoadAsync<T>(string path, UnityAction<T> callBack) where T : Object
        {
            MonoSystem.Start_Coroutine(DoLoadAsync<T>(path, callBack));
        }

        private static IEnumerator DoLoadAsync<T>(string path, UnityAction<T> callBack) where T: Object
        {
            var res = Resources.LoadAsync<T>(path);
            yield return res;
            callBack?.Invoke(res.asset as T);
        }
//#endif
        #endregion

        #region AssetBundle

#if PFC_RES_AB
        private static AssetBundleModule _assetBundleModule;

        private static AssetBundleModule AssetBundleModule
        {
            get
            {
                if (_assetBundleModule is not null) return _assetBundleModule;
                _assetBundleModule = new AssetBundleModule();
                return _assetBundleModule;
            }
        }

        public static AssetBundle LoadAssetBundle(string abName)
        {
            return AssetBundleModule.LoadAssetBundle(abName);
        }
        
        public static void UnLoadAssetBundle(string abName)
        {
           AssetBundleModule.UnLoadAssetBundle(abName);
        }

        public static void ClearAssetBundle()
        {
            AssetBundleModule.ClearAssetBundle();
        }
        
        public static T LoadResource<T>(string abName, string resName) where T : Object
        {
            return AssetBundleModule.LoadResource<T>(abName, resName);
        }

        public static object LoadResource(string abName, string resName,System.Type type)
        {
            return AssetBundleModule.LoadResource(abName, resName, type);
        }
        
        public static object LoadResource(string abName, string resName)
        {
            return AssetBundleModule.LoadResource(abName, resName);
        }

        public static void LoadResourceAsync<T>(string abName, string resName,UnityAction<T> callBack) where T: Object
        {
           AssetBundleModule.LoadResourceAsync<T>(abName,resName,callBack);
        }
        
        public static void LoadResourceAsync(string abName, string resName,UnityAction<Object> callBack)
        {
            AssetBundleModule.LoadResourceAsync(abName,resName,callBack);
        }
        
        public static void LoadResourceAsync(string abName, string resName,System.Type type,UnityAction<Object> callBack)
        {
            AssetBundleModule.LoadResourceAsync(abName,resName,type,callBack);
        }
#endif
        #endregion

    }
}