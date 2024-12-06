using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        private bool _createEvent = true;
        private UINode Target => target as UINode;
        private string ScriptPath => Application.dataPath + $"/PurpleFlowerCore/Scripts/UI/{Target.GetType().Namespace}/" +
                                      Target.GetType().Name + "_Gen.cs";
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
            // EditorGUILayout.LabelField("UINode子类必须定义为partial");
            Target.NodeName = EditorGUILayout.TextField("节点名称",Target.NodeName);
            _ignoreDefault = EditorGUILayout.Toggle("忽略默认名称节点",_ignoreDefault);
            _createEvent = EditorGUILayout.Toggle("生成UI事件",_createEvent);
            Target.TagColor = EditorGUILayout.ColorField("颜色标识",Target.TagColor);
        }

        public void CreateScript()
        {
            if (!GUILayout.Button("生成脚本")) return;
            PFCLog.Info("UINode","生成脚本");
            try
            {
                var type = Target.GetType();
                var uiBehaviours = GetUIBehaviours();
                var uiNodes = GetUINodes();
                List<string> fileLines = new();
                fileLines.Add(fillHead);
                fileLines.Add("using UnityEngine;");
                fileLines.Add("using UnityEngine.UI;");
                fileLines.Add("namespace " + type.Namespace);
                fileLines.Add("{");
                fileLines.Add("    public partial class " + type.Name);
                fileLines.Add("    {");
                foreach (var uiBehaviour in uiBehaviours)
                {
                    fileLines.Add("        [SerializeField] private " + uiBehaviour.Value.GetType().Name + " " +
                                  uiBehaviour.Key + ";");
                }
                foreach (var uiNode in uiNodes)
                {
                    fileLines.Add("        [SerializeField] private " + uiNode.Value.GetType().Name + " " +
                                  uiNode.Key + ";");
                }
                if(_createEvent)
                {
                    fileLines.Add("        protected override void InitEvent()");
                    fileLines.Add("        {");
                    foreach (var uiBehaviour in GetUIBehaviours<Button>())
                    {
                        fileLines.Add("            " + uiBehaviour.Key + $".onClick.AddListener({uiBehaviour.Key}Click);");
                    }
                    fileLines.Add("        }");
                }
                fileLines.Add("    }");
                fileLines.Add("}");
                
                HandleSourceScript();
                
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Join("\n",fileLines));
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

        private void HandleSourceScript()
        {
            var scripts = AssetDatabase.FindAssets($"{Target.GetType().Name} t:Script")
                .Select(AssetDatabase.GUIDToAssetPath);
            List<string> scriptPaths = new(scripts);
            scriptPaths = scriptPaths.FindAll(s => s.Contains("Asset"));
            scriptPaths.Remove(ScriptPath.Replace(Application.dataPath, "Assets"));
            if(scriptPaths.Count == 0)
            {
                PFCLog.Error("UINode","未找到脚本:" + Target.GetType().Name);
                throw new Exception();
            }
            var content = File.ReadAllText(scriptPaths[0]);
            List<string> lines = new(content.Split('\n'));

            for (int i = 0; i < lines.Count; i++)
            {
                if(lines[i].Contains(Target.GetType().Name))
                {
                    if (!lines[i].Contains("partial"))
                    {
                        lines[i] = lines[i].Replace("class", "partial class");
                    }
                    break;
                }
            }
            
            int startIndex = -1;
            int endIndex = -1;
            for (int i = 0; i < lines.Count; i++)
            {
                if(lines[i].Contains("#region UI Event"))
                {
                    if(startIndex != -1)
                    {
                        PFCLog.Error("UINode","there are more than one region UI Event");
                        return;
                    }
                    startIndex = i;
                }
                if (lines[i].Contains("#endregion"))
                {
                    if (endIndex != -1)
                    {
                        PFCLog.Error("UINode","there are no region UI Event");
                        return;
                    }
                    endIndex = i;
                }
            }
            
            if(startIndex == -1 || endIndex == -1)
            {
                content = string.Join("\n",lines);
                var subContent = content;
                int startIndex1 = subContent.IndexOf("class");
                subContent = subContent.Substring(startIndex1);
                int startIndex2 = subContent.IndexOf("{");
                startIndex2++;
                subContent = subContent.Substring(startIndex2);
                int num = 1;
                int index = 0;
                for (; index < subContent.Length; index++)
                {
                    if(subContent[index] == '{')
                        num++;
                    if (subContent[index] == '}')
                        num--;
                    if (num == 0)
                        break;
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("\n");
                sb.Append("        // Do not modify the region's name if you don't know how it works\n");
                sb.Append("        #region UI Event\n");
                var buttons = GetUIBehaviours<Button>();
                foreach (var button in buttons)
                {
                    sb.Append("        private void " + button.Key + "Click()\n");
                    sb.Append("        {\n");
                    sb.Append("            \n");
                    sb.Append("        }\n");
                }
                sb.Append("        #endregion\n    ");
                content = content.Insert(startIndex1 + startIndex2 + index, sb.ToString());
            }
            else
            {
                // for(int i = startIndex + 1; i < endIndex; i++)
                // {
                //     lines[i] = "//" + lines[i];
                // }

                startIndex++;
                var buttons = GetUIBehaviours<Button>();
                foreach (var button in buttons)
                {
                    if( lines.FindIndex(startIndex, endIndex - startIndex, s => s.Contains(button.Key)) != -1) continue;
                    lines.Insert(endIndex, "        private void " + button.Key + "Click()\n");
                    lines.Insert(endIndex + 1,"        {\n");
                    lines.Insert(endIndex + 2,"            \n");
                    lines.Insert(endIndex + 3,"        }\n");
                    endIndex += 4;
                }
                content = string.Join("\n",lines);
            }
            
            File.WriteAllText(scriptPaths[0], content);
                    lines.Insert(endIndex + 2,"        }\n");
        }

        private void DeleteScript()
        {
            if (!File.Exists(ScriptPath)) return;
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
        public Dictionary<string,UIBehaviour> GetUIBehaviours<T>() where T: UIBehaviour
        {
            var uiBehaviours = new Dictionary<string,UIBehaviour>();
            var allUIBehaviours = GetUIBehaviours();
            foreach (var uiBehaviour in allUIBehaviours)
            {
                if (uiBehaviour.Value.GetType() == typeof(T))
                {
                    uiBehaviours.Add(uiBehaviour.Key,uiBehaviour.Value);
                }
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