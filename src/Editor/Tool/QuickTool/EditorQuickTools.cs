using System;
using PurpleFlowerCore.Editor.Utility;
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
        
        [MenuItem("PFC/QuickTool", false, 302)]
        public static void CreateWindow()
        {
            var window = GetWindow<EditorQuickTools>();
            window.titleContent = new GUIContent("Quick Tool");
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
                    // button.color = new Color(button.color.r, button.color.g, button.color.b, 1);
                    GUI.backgroundColor = button.color;
                    if (GUILayout.Button(button.name))
                    {
                        try
                        {
                            if (button.commandType == QuickToolButtonData.CommandType.Custom)
                                button.command?.Invoke();
                            else
                                button.Command();
                        }
                        catch (Exception e)
                        {
                            PFCLog.Error("QuickTool", e.Message);
                        }
                    }
                    GUI.backgroundColor = Color.white;
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
