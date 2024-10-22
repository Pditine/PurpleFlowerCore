using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace PurpleFlowerCore.Utility
{
    public static class SOUtility
    {
        public static T GetSOByType<T>() where T : ScriptableObject
        {
            return GetSOByType(typeof(T)) as T;
        }
        
        public static ScriptableObject GetSOByType(Type type)
        {
            var objs = GetSOsByType(type);
            if(objs.Count>1)
                throw new Exception("There are more than one SOs of type " + type.Name);
            return objs[0];
        }
        
        public static void GetSOByName()
        {
            
        }

        public static List<ScriptableObject> GetSOsByType(Type type)
        {
            if(!type.IsSubclassOf(typeof(ScriptableObject)))
                throw new Exception("Type must be subclass of ScriptableObject");
            
            List<ScriptableObject> res = new();
            var guids = AssetDatabase.FindAssets($"t:{type.Name}");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (obj != null)
                {
                    res.Add(obj);
                }
            }
            return res;
        }
        

    }
}