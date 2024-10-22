using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
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
