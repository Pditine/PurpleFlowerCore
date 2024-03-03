using System;
using UnityEditor;
using UnityEngine;

namespace PurpleFlowerCore.Editor
{
    //[CreateAssetMenu(fileName = "PFCSetting",menuName = "PFC/PFCSetting")]
    [Obsolete("暂时不使用",false)]
    public class PFCSetting : ScriptableObject
    {
        // [Header("数据持久化方式")] public SaveState saveState;
        // [Header("资源管理方式")] public ResourceState resourceState;
        
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
}