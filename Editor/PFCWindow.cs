using System;
using UnityEditor;
using UnityEngine;

namespace PurpleFlowerCore.Editor
{
    public class PFCWindow: EditorWindow
    {
        private SaveMode _saveMode;
        private SaveMode _newSaveMode;
        
        [MenuItem("PFC/打开菜单")]
        private static void OpenWindow()
        {
            EditorWindow win = GetWindow<PFCWindow>("Purple Flower Core");
            win.Show();
            win.maxSize = new Vector2(800, 600);
            win.maximized = true;
        }

        private void OnEnable()
        {
            _newSaveMode = _saveMode;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("欢迎使用PurpleFlowerCore，这是我为了更好的个性化而制作的程序框架和工具集合",GUILayout.Height(100));
            _newSaveMode = (SaveMode)EditorGUILayout.EnumPopup(_newSaveMode);
            if (GUILayout.Button("应用"))
            {
                ChangeSaveState();
            }
            
        }

        private void ChangeSaveState()
        {
            RemoveScriptCompilationSymbol("PFC_SAVE_" + _saveMode.ToString().ToUpper());
            _saveMode = _newSaveMode;
            AddScriptCompilationSymbol("PFC_SAVE_" + _saveMode.ToString().ToUpper());
        }
        
        /// <summary>
        /// 增加预处理指令
        /// </summary>
        private static void AddScriptCompilationSymbol(string name)
        {
            BuildTargetGroup buildTargetGroup = UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup;
            string group = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if (!group.Contains(name))
            {
                UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, group + ";" + name);
            }
        }

        /// <summary>
        /// 移除预处理指令
        /// </summary>
        private static void RemoveScriptCompilationSymbol(string name)
        {
            BuildTargetGroup buildTargetGroup = UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup;
            string group = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if (group.Contains(name))
            {
                UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, group.Replace(";" + name, string.Empty));
            }
        }
    }
    
    public enum SaveMode
    {
        Json,LitJson,Binary
    }

    public enum ResourceMode
    {
        Resources,AssetBundle
    }
}