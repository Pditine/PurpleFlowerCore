using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PurpleFlowerCore.PFCDebug
{
    public class DebugMenu : MonoBehaviour, IDragHandler
    {
        //todo:UI框架
        [SerializeField] private GameObject menu;
        [SerializeField] private Transform itemRoot;
        [SerializeField] private LogPanel logPanel;
        
        private readonly Tree<Action> _commandTree = new();
        // private readonly Dictionary<string,DebugCommandInfo> _menuCommands = new();
        private readonly List<DebugMenuItem> _items = new();
        private TreeNode<Action> _currentNode;
        // [Inspectable]private int _currentPathIndex = 0;
        private const string ItemUIPath = "PFCRes/DebugMenuItem";
        
        public void AddCommand(string commandPath, Action command)
        {
            _commandTree.CreateNodeByPath(commandPath, command);
        }

        private void Awake()
        {
            _currentNode = _commandTree.Root;
        }

        public void ClickItem(TreeNode<Action> commandNode)
        {
            if (commandNode.IsLeaf)
            {
                PFCLog.Debug("DebugMenu", "execute command:" + commandNode.name);
                commandNode.Value?.Invoke();
            }
            else
            {
                _currentNode = commandNode;
                Refresh();
            }
        }

        private void ClearItems()
        {
            foreach (var item in _items)
            {
                Destroy(item.gameObject);
            }
            _items.Clear();
        }
        
        public void Back()
        {
            if (_currentNode.IsRoot) return;
            _currentNode = _currentNode.Parent;
            Refresh();
        }
        
        private void ShowItem(TreeNode<Action> commandNode)
        {
            var newItem = Instantiate(Resources.Load<DebugMenuItem>(ItemUIPath), itemRoot);
            _items.Add(newItem);
            newItem.Init(commandNode, this);
        }

        private void ShowItems()
        {
            foreach (var node in _currentNode.Children)
            {
                ShowItem(node);
            }
        }
        
        public void Switch()
        {
            Switch(!menu.activeSelf);
        }

        public void Switch(bool open)
        {
            menu.SetActive(open);
            if (open)
            {
                Refresh();
            }
        }
        
        public void Refresh()
        {
            ClearItems();
            ShowItems();
        }
        
        public void Log(LogData data)
        {
            logPanel.Print(data);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position += (Vector3)eventData.delta;
        }
    }
}