using System;
using System.Collections.Generic;
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
    }

    [Serializable]
    public class QuickToolButtonData
    {
        public string name;
        public bool lineBreak;
        public UnityEvent command;
    }

}
