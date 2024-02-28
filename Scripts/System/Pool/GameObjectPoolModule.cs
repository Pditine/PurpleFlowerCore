using System.Collections.Generic;
using UnityEngine;

namespace PurpleFlowerCore.Pool
{
    public class GameObjectPoolModule : MonoBehaviour
    {
        private readonly Dictionary<string,GameObjectPoolData> _data = new();

        public void InitGameObjectPoolData(GameObject theGameObject,int maxCount = -1, bool infinitePop = true, bool fillWhenInit = false)
        {
            CheckGameObjectPoolData(theGameObject);
            _data[theGameObject.name].Init(maxCount,infinitePop,fillWhenInit);
        }
        
        public void InitGameObjectPoolData(string theGameObjectName,int maxCount = -1, bool infinitePop = true, bool fillWhenInit = false)
        {
            if(CheckGameObjectPoolData(theGameObjectName))
                _data[theGameObjectName].Init(maxCount,infinitePop,fillWhenInit);
        }
        
        public GameObject GetGameObject(GameObject theGameObject)
        {
            CheckGameObjectPoolData(theGameObject);
            return _data[theGameObject.name].Pop();
        }
        
        public GameObject GetGameObject(string theGameObjectName)
        {
            if(CheckGameObjectPoolData(theGameObjectName))
                return _data[theGameObjectName].Pop();
            return null;
        }
        
        public void PushGameObject(GameObject theGameObject)
        {
            CheckGameObjectPoolData(theGameObject);
            _data[theGameObject.name].Push(theGameObject);
        }
        
        private void CheckGameObjectPoolData(GameObject theGameObject)
        {
            string gameObjectName = theGameObject.name;
            if (_data.ContainsKey(gameObjectName)) return;
            var root = new GameObject(gameObjectName+"Pool")
            {
                transform =
                {
                    parent = gameObject.transform
                }
            };
            _data.Add(gameObjectName,new GameObjectPoolData(root.transform,theGameObject));
            _data[gameObjectName].Init();
        }

        private bool CheckGameObjectPoolData(string theGameObjectName)
        {
            if (_data.ContainsKey(theGameObjectName)) return true;
            PFCLog.Error("请提供有物体为参数的对象池数据初始化:"+theGameObjectName);
            return false;
            // var root = new GameObject(theGameObjectName+"Pool")
            // {
            //     transform =
            //     {
            //         parent = gameObject.transform
            //     }
            // };
            //todo:查找资源,希望使用某种资源管理方式自动提供物体
            //_data.Add(theGameObjectName,new GameObjectPoolData(root.transform,theGameObjectName));
        }
    }
}