using System;
using System.Collections.Generic;

namespace PurpleFlowerCore.Pool
{
    public class ObjectPoolModule
    {
        private readonly Dictionary<string,ObjectPoolData> _data = new();

        //public void InitObjectPoolData<T>(int maxCount = -1, bool infinitePop = true, bool fillWhenInit = false)
        public void InitObjectPoolData<T>(int maxCount = -1)
        {
            CheckObjectPoolData<T>();
            //_data[typeof(T).FullName].Init(maxCount,infinitePop,fillWhenInit);
            _data[typeof(T).FullName].Init(maxCount);
        }
        
        //public void InitObjectPoolData(Type type, int maxCount = -1, bool infinitePop = true, bool fillWhenInit = false)
        public void InitObjectPoolData(Type type, int maxCount = -1)
        {
            CheckObjectPoolData(type);
            //_data[type.FullName].Init(maxCount,infinitePop,fillWhenInit);
            _data[type.FullName].Init(maxCount);
        }
        
        public T GetObject<T>() where T: class
        {
            InitObjectPoolData<T>();
            return _data[typeof(T).FullName].Pop() as T;
        }
        
        public object GetObject(Type type)
        {
            InitObjectPoolData(type);
            return _data[type.FullName].Pop();
        }
        
        public void PushGameObject<T>(T theObject)
        {
            InitObjectPoolData<T>();
            _data[typeof(T).FullName].Push(theObject);
        }
        
        public void PushGameObject(Type type,object theObject)
        {
            InitObjectPoolData(type);
            _data[type.FullName].Push(theObject);
        }
        
        private void CheckObjectPoolData<T>()
        {
            string objectName = typeof(T).FullName;
            if (_data.ContainsKey(objectName)) return;
            _data.Add(objectName,new ObjectPoolData(typeof(T)));
            _data[objectName].Init();
        }

        private void CheckObjectPoolData(Type type)
        {
            string objectName = type.FullName;
            if (_data.ContainsKey(objectName)) return;
            _data.Add(objectName,new ObjectPoolData(type));
            _data[objectName].Init();
        }
        
    }
}