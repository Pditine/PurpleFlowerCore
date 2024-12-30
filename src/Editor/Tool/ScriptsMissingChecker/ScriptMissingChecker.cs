using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using GameObjectUtility = UnityEditor.GameObjectUtility;

namespace PurpleFlowerCore.Editor.Tool
{
    public class ScriptMissingChecker: EditorWindow
    {
        private readonly List<string> _filter = new();
        private readonly List<GameObject> _gos = new();
        private bool _showGameObject = true;
        private bool _showFilter;
        private string _path = "Assets/";
        private Vector2 _scrollPosition;
        
        [MenuItem("GP/Script Missing Checker")]
        private static void OpenWindow()
        {
            var win = GetWindow<ScriptMissingChecker>("Script Missing Checker");
            win.Show();
        }
        
        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.LabelField("这是一个检查是否有物体丢失脚本的工具，点击CheckScene检" +
                                     "查场景中的物体，点击CheckAsset检查资源中的物体。可以在" +
                                     "Filter中输入需要忽略的文件夹路径，点击RemoveMissingScripts" +
                                     "将去除GameObjects中的Missing脚本", EditorStyles.wordWrappedLabel);
            _path = EditorGUILayout.TextField("Path", _path);
            _showFilter = EditorGUILayout.Foldout(_showFilter, "Filter");
            if(_showFilter)
            {
                for (int i = 0; i < _filter.Count; i++)
                {
                    _filter[i] = EditorGUILayout.TextField(_filter[i]);
                }
                if (GUILayout.Button("Add Filter"))
                {
                    _filter.Add("");
                }
            }
            _showGameObject = EditorGUILayout.Foldout(_showGameObject, "GameObjects");
            if(_showGameObject)
            {
                foreach (var go in _gos)
                {
                    EditorGUILayout.ObjectField(go, typeof(GameObject), true);
                }
                RemoveMissingScripts();
            }
            CheckAsset();
            CheckScene();
            EditorGUILayout.EndScrollView();
        }
        
        private void CheckScene()
        {
            if (!GUILayout.Button("Check Scene")) return;
            _gos.Clear();
            StringBuilder sb = new StringBuilder();
            GameObject[] gos = FindObjectsOfType<GameObject>();
            foreach (var go in gos)
            {
                //Component[] components = go.GetComponents<Component>();
                // if (components.Any(component => component == null))
                // {
                //     _gos.Add(go);
                //     sb.AppendLine(go.GetScenePath());
                // }
                
                if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go) > 0)
                {
                    _gos.Add(go);
                    sb.AppendLine(go.ToString());
                }
            }
            if(sb.Length == 0)
            {
                Debug.Log("[ScriptMissingChecker] No missing scripts found in Scene.");
                return;
            }
            
            Debug.LogError($"[ScriptMissingChecker] Found missing scripts in Scene:\n{sb}");
        }
        
        private void CheckAsset()
        {
            if (!GUILayout.Button("Check Asset")) return;
            _gos.Clear();
            StringBuilder sb = new StringBuilder();
            AssetDatabase.FindAssets("t:GameObject",new []{_path}).ToList().ForEach(guid =>
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if(_filter.Count > 0 && _filter.Any(filter => !string.IsNullOrEmpty(filter) && path.StartsWith(filter))) return;
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                // Component[] components = go.GetComponents<Component>();
                // if (components.Any(component => component == null))
                // {
                //
                // }
                int num = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
                if (num > 0)
                {
                    _gos.Add(go);
                    sb.AppendLine(path);
                    
                }
            });
            if(sb.Length == 0)
            {
                Debug.Log("[ScriptMissingChecker] No missing scripts found in Assets.");
                return;
            }
            
            Debug.LogError($"[ScriptMissingChecker] Found missing scripts in Prefabs:\n{sb}");
        }

        private void RemoveMissingScripts()
        {
            if (!GUILayout.Button("Remove Missing Scripts")) return;
            StringBuilder sb = new();
            foreach (var go in _gos)
            {
                var num = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                if (num > 0)
                {
                    sb.AppendLine($"{go.name}: {num} scripts");
                }
            }
            if (sb.Length == 0)
            {
                Debug.Log("[ScriptMissingChecker] No missing scripts found.");
                return;
            }
            Debug.Log($"[ScriptMissingChecker] Removed missing scripts in GameObjects:\n{sb}");
        }
    }
}