using UnityEditor;
using UnityEngine;

namespace PurpleFlowerCore.Editor.Tool
{
    [CustomPropertyDrawer(typeof(QuickToolButtonData))]
    public class QuickToolButtonDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            // var indent = EditorGUI.indentLevel;
            // EditorGUI.indentLevel = 1;
            var target = property.boxedValue as QuickToolButtonData;
            
            EditorGUILayout.PropertyField(property.FindPropertyRelative("name"), new GUIContent("Name"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("lineBreak"), new GUIContent("Line Break"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("color"), new GUIContent("Color"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("commandType"), new GUIContent("Command Type"));
            if(target is { commandType: QuickToolButtonData.CommandType.Custom })
            {
                EditorGUILayout.PropertyField(property.FindPropertyRelative("command"), new GUIContent("Command"));
            }
            
            else
                EditorGUILayout.PropertyField(property.FindPropertyRelative("commandParam"));
            // EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }
    }
}