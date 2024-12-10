using System;
using System.Collections.Generic;
using PurpleFlowerCore.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace PurpleFlowerCore.Editor.Tool
{
    public class QuickToolConfig : ScriptableObject
    {
        public List<QuickToolButtonData> quickToolButtonData = new();

        public static void OpenScene(string scenePath)
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath);
        }
        
        public static void OpenFolder(string path)
        {
            var obj = AssetDatabase.LoadAssetAtPath(path,typeof(UnityEngine.Object));
            EditorGUIUtility.PingObject(obj);
            AssetDatabase.OpenAsset(obj);
        }
    }

    [Serializable]
    public class QuickToolButtonData
    {
        public string name;
        public CommandType commandType;
        public bool lineBreak;
        public string commandParam;
        public UnityEvent command;
    }

    public enum CommandType
    {
        OpenScene,
        OpenFolder,
        Custom
    }
}
