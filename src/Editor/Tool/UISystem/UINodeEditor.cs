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
        private bool _ignoreDefault = true;
        private UINode Target => target as UINode;
        private string ScriptPath => Application.dataPath + $"/PurpleFlowerCore/Scripts/UI/{Target.GetType().Namespace}/" +
                                      Target.GetType().Name + ".cs";
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(5);
            Property();
            GUI.backgroundColor = new Color(0.7f,0.3f,0.7f);
            if(Application.isPlaying) return;
            CreateScript();
            Refresh();
            DeleteScript();
        }

        public void Property()
        {
            EditorGUILayout.LabelField("UINode子类必须定义为partial");
            Target.NodeName = EditorGUILayout.TextField("节点名称",Target.NodeName);
            _ignoreDefault = EditorGUILayout.Toggle("忽略默认名称节点",_ignoreDefault);
            Target.TagColor = EditorGUILayout.ColorField("颜色标识",Target.TagColor);
        }

        public void CreateScript()
        {
            if (!GUILayout.Button("生成脚本")) return;
            PFCLog.Info("UINode","生成脚本");
            try
            {
                var type = Target.GetType();
                // var filePath = AssetDatabase.GetAssetPath(Target);
                // var fileStr = File.ReadAllText(filePath);
                // PFCLog.Info(fileStr);
                var uiBehaviours = GetUIBehaviours();
                var uiNodes = GetUINodes();
                StringBuilder sb = new StringBuilder();
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
                foreach (var uiNode in uiNodes)
                {
                    sb.Append("        [SerializeField] private " + uiNode.Value.GetType().Name + " " +
                              uiNode.Key + ";\n");
                }
                sb.Append("    }\n");
                sb.Append("}\n");
                //todo: 文件资源管理
                if(!Directory.Exists(Path.GetDirectoryName(ScriptPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(ScriptPath));
                }
                File.WriteAllText(ScriptPath, sb.ToString());
                AssetDatabase.Refresh();
                PFCLog.Info("UINode","生成成功:" + ScriptPath);
            }
            catch (Exception e)
            {
                PFCLog.Error("UINode","生成失败:" + e.Message);
                throw;
            }
        }

        private void DeleteScript()
        {
            if (!GUILayout.Button("删除脚本")) return;
            if (File.Exists(ScriptPath))
            {
                File.Delete(ScriptPath);
                AssetDatabase.Refresh();
                PFCLog.Info("UINode","删除成功:" + ScriptPath);
            }
            else
            {
                PFCLog.Error("UINode","文件不存在:" + ScriptPath);
            }
        }
        
        private void Refresh()
        {
            if (!GUILayout.Button("刷新")) return;
            var uiBehaviours = GetUIBehaviours();
            var uiNodes = GetUINodes();
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
            foreach (var uiNode in uiNodes)
            {
                var field = Target.GetType().GetField(uiNode.Key,BindingFlags.NonPublic | BindingFlags.Instance);
                if(field == null)
                {
                    PFCLog.Error("UINode","字段不存在:" + uiNode.Key);
                    continue;
                }
                field.SetValue(Target,uiNode.Value);
            }
        }
        
        public Dictionary<string,UIBehaviour> GetUIBehaviours()
        {
            var uiBehaviours = new Dictionary<string,UIBehaviour>();
            var uiBehavioursArray = Target.GetComponentsInChildren<UIBehaviour>(true);
            foreach (var uiBehaviour in uiBehavioursArray)
            {
                if(uiBehaviour.GetComponentInParent<UINode>() != Target) continue;
                if(_ignoreDefault && _nameFilter.Contains(uiBehaviour.name))continue;
                uiBehaviours.Add(uiBehaviour.name.Replace(" ","") + uiBehaviour.GetType().Name,uiBehaviour);
            }
            return uiBehaviours;
        }

        public Dictionary<string, UINode> GetUINodes()
        {
            var uiNodes = new Dictionary<string, UINode>();
            List<UINode> uiNodeList = new(Target.GetComponentsInChildren<UINode>(true));
            uiNodeList.Remove(Target);
            foreach (var uiNode in uiNodeList)
            {
                if(uiNode.transform.parent.GetComponentInParent<UINode>() != Target) continue;
                string NodeName = string.IsNullOrEmpty(uiNode.NodeName) ? uiNode.name : uiNode.NodeName;
                NodeName = NodeName.Replace(" ", "");
                uiNodes.Add(NodeName,uiNode);
            }
            return uiNodes;
        }
    }
}