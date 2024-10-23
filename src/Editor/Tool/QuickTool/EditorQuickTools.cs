using System;
using PurpleFlowerCore.Utility;
using UnityEditor;
using UnityEngine;

namespace PurpleFlowerCore.Editor.Tool
{
    #region 便捷工具箱
    public sealed class EditorQuickTools : EditorWindow
    {
        private QuickToolConfig _config;
        private QuickToolConfig Config
        {
            get
            {
                if (_config == null)
                {
                    _config = SOUtility.GetSOByType<QuickToolConfig>(true);
                }
                return _config;
            }
        }

        private bool _openConfigPanel;
        private UnityEditor.Editor _configPanel;
        private UnityEditor.Editor ConfigPanel
        {
            get
            {
                if(_configPanel == null)
                    _configPanel = UnityEditor.Editor.CreateEditor(Config);
                return _configPanel;
            }
        }

        private Vector2 _scrollPosition;
        
        [MenuItem("PFC/Tool/QuickTool", false, 302)]
        public static void CreateWindow()
        {
            var window = GetWindow<EditorQuickTools>();
            window.titleContent = new GUIContent("Quick Tools");
            window.minSize = new Vector2(150f, 50f);
            window.Show();
        }

        private void OnGUI()
        {
            try
            {
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                foreach (var button in Config.quickToolButtonData)
                {
                    if (button.lineBreak)
                    {
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                    }

                    if (GUILayout.Button(button.name))
                    {
                        button.command?.Invoke();
                    }
                }

                EditorGUILayout.EndHorizontal();


                _openConfigPanel = EditorGUILayout.Toggle("打开配置面板", _openConfigPanel);
                if (_openConfigPanel)
                    ConfigPanel.OnInspectorGUI();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
            }            
            catch (Exception e)
            {
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
                var style = new GUIStyle(EditorStyles.boldLabel) {normal = {textColor = Color.red}};
                EditorGUILayout.LabelField(e.Message,style);
                throw;
            }
        }
    }

    #endregion
}
