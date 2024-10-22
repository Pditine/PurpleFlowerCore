using LilithGames.Party;
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
                    _config = SOUtility.GetSOByType<QuickToolConfig>();
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
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Open C# Project"))
            {
                EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
            }

            if (GUILayout.Button("Refresh"))
            {
                EditorApplication.ExecuteMenuItem("Assets/Refresh");
            }

            if (GUILayout.Button("Clear Console"))
            {
                QuickToolsHotKey.ClearConsole();
            }

            // if (GUILayout.Button("Open Config File"))
            // {
            //     string path = Application.persistentDataPath.Replace("/","\\");
            //     System.Diagnostics.Process.Start("explorer.exe", path);
            // }

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
            if(_openConfigPanel)
                ((UnityEditor.Editor)ConfigPanel).OnInspectorGUI(); // 这里必须要强转，否则会报错
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }

    #endregion
}
