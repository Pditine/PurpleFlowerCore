using System.Collections.Generic;
using UnityEngine;

namespace PurpleFlowerCore.Config
{
    public abstract class JsonConfigData<T> : ConfigData where T : class
    {
        protected abstract string GetLoadPath();
        private Dictionary<string,T> _data;
        public T this[string key] => GetItem(key);
        public T GetItem(string key)
        {
            if (_data == null)
            {
                Load();
            }
            return _data[key];
        }

        public void Load()
        {   
            _data = new Dictionary<string, T>();
            var path = GetLoadPath();
            var json = System.IO.File.ReadAllText(path);
            var data = JsonUtility.FromJson<Dictionary<string, T>>(json);
            foreach (var item in data)
            {

                _data.Add(item.Key, item.Value);
            }
        }
    }
}
