using System;
using System.Collections.Generic;
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
        
        public static void OpenCSProject()
        {
            PFCLog.Info(123);
            EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
        }
        
        public static void Refresh()
        {
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
        }
        
        public static void ClearConsole()
        {
            QuickToolsHotKey.ClearConsole();
        }
        
        public static void ProjectFolder(string path)
        {
            var obj = AssetDatabase.LoadAssetAtPath(path,typeof(UnityEngine.Object));
            EditorGUIUtility.PingObject(obj);
            AssetDatabase.OpenAsset(obj);
            PFCLog.Info("QuickTool", "Open Project Folder: " + path);
        }
    }

    [Serializable]
    public class QuickToolButtonData
    {
        public string name;
        public bool lineBreak;
        public UnityEvent command;

        // public QuickToolButtonData()
        // {
        //     this.name = "指令名称";
        //     this.lineBreak = false;
        //     this.command = null;
        // }

    }

}
