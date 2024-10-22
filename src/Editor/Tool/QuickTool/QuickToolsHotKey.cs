

using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace LilithGames.Party
{
    #region HotKey

    public static class QuickToolsHotKey
    {
        [MenuItem("PFC/Tool/HotKey/Clear Console &c", false, 300)]
        public static void ClearConsole()
        {
            var assembly = Assembly.GetAssembly(typeof(SceneView));
            var logEntries = assembly.GetType("UnityEditor.LogEntries");
            var method = logEntries.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            if (method != null)
            {
                method.Invoke(null, null);
                GUIUtility.keyboardControl = 0;
            }
            else
            {
                Debug.LogWarning("Can't find clear method");
            }
        }

        [MenuItem("PFC/Tool/HotKey/Align View to MainCamera &#f", false, 301)]
        public static void AlignViewToMainCamera()
        {
            if (SceneView.lastActiveSceneView != null && Camera.main != null)
            {
                SceneView.lastActiveSceneView.orthographic = true;
                SceneView.lastActiveSceneView.AlignViewToObject(Camera.main.transform);
            }
        }
    }

    #endregion
}