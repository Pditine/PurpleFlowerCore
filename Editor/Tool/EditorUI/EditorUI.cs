using UnityEditor;
using UnityEngine;

namespace PurpleFlowerCore.Editor.Tool
{
    public static class EditorUI
    {
        public static void ShowInt(ref int value, string label)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(100));
            value = EditorGUILayout.IntField(value);
            GUILayout.EndHorizontal();   
        }
    }
    
    
}