using System;
using PurpleFlowerCore.Base;
using UnityEngine;
using PurpleFlowerCore.Pool;
namespace PurpleFlowerCore
{
    public static class PoolSystem
    {
        private static ObjectPoolModule _objectPoolModule;
        private static ObjectPoolModule ObjectPoolModule
        {
            get
            {
                if (_objectPoolModule is null)
                    _objectPoolModule = new ObjectPoolModule();
                return _objectPoolModule;
            }
            
        }

        private static GameObjectPoolModule _gameObjectPoolModule;
        private static GameObjectPoolModule GameObjectPoolModule
        {
            get
            {
                if (_gameObjectPoolModule is not null) return _gameObjectPoolModule;
                var root = new GameObject("Pool")
                {
                    transform = { parent = PFCManager.Instance.transform }
                };
                _gameObjectPoolModule = root.AddComponent<GameObjectPoolModule>();
                return _gameObjectPoolModule;
            }
        }

        #region GameObjectPool

        public static void InitGameObjectPoolData(GameObject theGameObject,int maxCount = -1, bool infinitePop = true, bool fillWhenInit = false)
        {
            GameObjectPoolModule.InitGameObjectPoolData(theGameObject,maxCount,infinitePop,fillWhenInit);
        }
        
        public static void InitGameObjectPoolData(string theGameObjectName,int maxCount = -1, bool infinitePop = true, bool fillWhenInit = false)
        {
            GameObjectPoolModule.InitGameObjectPoolData(theGameObjectName,maxCount,infinitePop,fillWhenInit);
        }
        
        public static GameObject GetGameObject(GameObject theGameObject)
        {
            return GameObjectPoolModule.GetGameObject(theGameObject);
        }
        
        public static GameObject GetGameObject(string theGameObjectName)
        {
            return GameObjectPoolModule.GetGameObject(theGameObjectName);
        }
        
        public static void PushGameObject(GameObject theGameObject)
        {
            GameObjectPoolModule.PushGameObject(theGameObject);
        }

        #endregion

        #region ObjectPool

        public static void InitObjectPoolData<T>(int maxCount = -1, bool infinitePop = true, bool fillWhenInit = false)
        {
            //ObjectPoolModule.InitObjectPoolData<T>(maxCount,infinitePop,fillWhenInit);
            ObjectPoolModule.InitObjectPoolData<T>(maxCount);
        }
        
        public static void InitObjectPoolData(Type type, int maxCount = -1, bool infinitePop = true, bool fillWhenInit = false)
        {
            //ObjectPoolModule.InitObjectPoolData(type,maxCount,infinitePop,fillWhenInit);
            ObjectPoolModule.InitObjectPoolData(type,maxCount);
        }
        
        public static T GetObject<T>() where T: class
        {
            return ObjectPoolModule.GetObject<T>();
        }
        
        public static object GetObject(Type type)
        {
            return ObjectPoolModule.GetObject(type);
        }
        
        public static void PushGameObject<T>(T theObject)
        {
            ObjectPoolModule.PushGameObject<T>(theObject);
        }
        
        public static void PushGameObject(Type type,object theObject)
        {
            ObjectPoolModule.PushGameObject(type,theObject);
        }

        #endregion
    }
}