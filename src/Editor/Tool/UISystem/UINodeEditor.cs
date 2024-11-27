using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PurpleFlowerCore.Editor.Tool.UISystem
{
    [CustomEditor(typeof(UINode), true)]
    public class UINodeEditor : UnityEditor.Editor
    {
        private string[] _nameFilter = {"Image","Text","Button","InputField","Toggle","Slider","Scrollbar","Dropdown",
            "ScrollRect","Mask","RawImage","Button (Legacy)", "Text (Legacy)",};
        private string fillHead = "// This script is auto-generated by UINodeEditor, which is a tool given by PurpleFlowerCore\n// Please do not modify this script manually\n" +
                                  "// url: https://github.com/Pditine/PurpleFlowerCore\n";
        private UINode Target => target as UINode;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(5);
            Property();
            GUI.backgroundColor = new Color(0.7f,0.3f,0.7f);
            CreateScriptButton();
            RefreshButton();
        }

        public void Property()
        {
            Target.NodeName = EditorGUILayout.TextField("节点名称",Target.NodeName);
            Target.TagColor = EditorGUILayout.ColorField("颜色标识",Target.TagColor);
        }

        private void CreateScriptButton()
        {
            if (!GUILayout.Button("生成脚本")) return;
            CreateScript();
        }
        
        private void RefreshButton()
        {
            if (!GUILayout.Button("刷新")) return;
            Refresh();
        }

        public void CreateScript()
        {
            PFCLog.Info("UINode","生成脚本");
            try
            {
                var uiBehaviours = GetUIBehaviours();
                StringBuilder sb = new StringBuilder();
                var type = Target.GetType();
                sb.Append(fillHead);
                sb.Append("using UnityEngine;\n");
                sb.Append("using UnityEngine.UI;\n");
                sb.Append("namespace " + type.Namespace + "\n");
                sb.Append("{\n");
                sb.Append("    public partial class " + type.Name + "\n");
                sb.Append("    {\n");
                foreach (var uiBehaviour in uiBehaviours)
                {
                    sb.Append("        [SerializeField] private " + uiBehaviour.Value.GetType().Name + " " +
                              uiBehaviour.Key + ";\n");
                }

                sb.Append("    }\n");
                sb.Append("}\n");
                //todo: 文件资源管理
                var path = Application.dataPath + $"/PurpleFlowerCore/Scripts/UI/{type.Namespace}/" +
                           type.Name + ".cs";
                if(!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                File.WriteAllText(path, sb.ToString());
                AssetDatabase.Refresh();
                PFCLog.Info("UINode","生成成功:" + path);
            }
            catch (Exception e)
            {
                PFCLog.Error("UINode","生成失败:" + e.Message);
                throw;
            }
        }
        
        private void Refresh()
        {
            var uiBehaviours = GetUIBehaviours();
            foreach (var uiBehaviour in uiBehaviours)
            {
                var field = Target.GetType().GetField(uiBehaviour.Key,BindingFlags.NonPublic | BindingFlags.Instance);
                
                if(field == null)
                {
                    PFCLog.Error("UINode","字段不存在:" + uiBehaviour.Key);
                    continue;
                }
                field.SetValue(Target,uiBehaviour.Value);
            }
        }
        
        public Dictionary<string,UIBehaviour> GetUIBehaviours()
        {
            var uiBehaviours = new Dictionary<string,UIBehaviour>();
            var uiBehavioursArray = Target.GetComponentsInChildren<UIBehaviour>();
            foreach (var uiBehaviour in uiBehavioursArray)
            {
                if(uiBehaviour.GetComponentInParent<UINode>() != Target) continue;
                if(_nameFilter.Contains(uiBehaviour.name))continue;
                uiBehaviours.Add(uiBehaviour.name + uiBehaviour.GetType().Name,uiBehaviour);
            }
            return uiBehaviours;
        }
    }
}