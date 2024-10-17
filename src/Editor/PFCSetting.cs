﻿using System.IO;
using UnityEditor;
using UnityEngine;

namespace PurpleFlowerCore.Editor
{
    public class PFCSetting
    {
        private static PFCSetting _instance;
        public static PFCSetting Instance
        {
            get
            {
                if (_instance is not null) return _instance;
                if (!File.Exists(Path + "setting.json"))ReSet(); 
                var jsonStr = File.ReadAllText(Path+"setting.json");
                _instance = JsonUtility.FromJson<PFCSetting>(jsonStr);
                AssetDatabase.Refresh();
                return _instance;
            }
        }
        private static string Path => Application.persistentDataPath + "/Setting/";
        
        public SaveMode SaveMode;
        //public ResourceMode ResourceMode;
        public bool LogInfo;
        public bool LogWarning;
        public bool LogError;
        public bool HasStarted;

        public static void ReSet()
        {
            if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
            if (!File.Exists(Path + "setting.json")) File.Create(Path + "setting.json");
            if (_instance == null)
            {
                _instance = new PFCSetting();
            }
            File.WriteAllText(Path+"setting.json", JsonUtility.ToJson(_instance));
            
        }

        // [MenuItem("PFC/创建默认json文件")]
        // private static void CreateDefaultSettingJson() // 仅开发中使用
        // {
        //     var defaultSettingJson = new PFCSetting();
        //     defaultSettingJson.SaveMode = SaveMode.Json;
        //     defaultSettingJson.LogInfo = false;
        //     defaultSettingJson.LogWarning = false;
        //     defaultSettingJson.LogError = false;
        //     var jsonStr = JsonUtility.ToJson(defaultSettingJson);
        //     File.WriteAllText(Path+"setting.json", jsonStr);
        //     AssetDatabase.Refresh();
        // }
        
    }
    
    public enum SaveMode
    {
        Json,LitJson,Binary
    }

    public enum ResourceMode
    {
        Resources,AssetBundle
    }
}