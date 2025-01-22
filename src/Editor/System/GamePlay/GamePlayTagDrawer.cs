using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GP.GameplayKit
{
    [CustomPropertyDrawer(typeof(GamePlayTagAttribute))]
    public class GamePlayTagDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string catalogue = ((GamePlayTagAttribute)attribute).Catalogue;
            //var tags = GetTagsInCatalogue(catalogue);
            if (string.IsNullOrEmpty(catalogue))
            {
                catalogue = "GamePlayTag";
            }

            GenericMenu gamePlayTagMenu = new GenericMenu();
            // foreach (var tag in tags)
            // {
            //     gamePlayTagMenu.AddItem(new GUIContent(tag), false, () =>
            //     {
            //         property.stringValue = tag;
            //         property.serializedObject.ApplyModifiedProperties();
            //     });
            // }

            EditorGUILayout.LabelField(label);
            EditorGUILayout.BeginHorizontal();
            var content = new GUIContent(catalogue);
            EditorGUILayout.TextField(property.stringValue);
            if (EditorGUILayout.DropdownButton(content, FocusType.Passive))
            {
                gamePlayTagMenu.DropDown(new Rect(position.x, position.y + 35, 10, 10));
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);
            // }
        }
        
        // private List<string> GetTagsInCatalogue(string catalogue)
        // {
        //     var rows = GameplayTagTable.Instance.GetAll();
        //     if (string.IsNullOrEmpty(catalogue))
        //     {
        //         return rows.Select(row => row.Value.Value).ToList();
        //     }
        //     List<string> tags = new List<string>();
        //     foreach (var row in rows)
        //     {
        //         var tagValue = row.Value.Value;
        //         if (tagValue.StartsWith(catalogue))
        //         {
        //             tags.Add(tagValue);
        //         }
        //     }
        //     return tags;
        // }
    }
}