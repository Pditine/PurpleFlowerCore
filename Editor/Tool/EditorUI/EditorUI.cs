using UnityEditor;
using UnityEngine;

namespace PurpleFlowerCore.Editor.Tool
{
    public static class EditorUI
    {
        public static void ShowInt(string label, ref int value, GUIStyle style = null, params GUILayoutOption[] option)
        {
            GUILayout.Label(label, GUILayout.Width(100));
            value = EditorGUILayout.IntField(label, value, style, option);
        }
    }
    
    public class TestWindow : EditorWindow
    {
        private int _testValue = 0;
        
        [MenuItem("PFC/TestWindow")]
        public static void ShowWindow()
        {
            GetWindow<TestWindow>("TestWindow");
        }

        private void OnGUI()
        {
            EditorUI.ShowInt("Test Value", ref _testValue);
            PFCLog.Info("TestWindow" + _testValue);
        }
    }
    
    
}