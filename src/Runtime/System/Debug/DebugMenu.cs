using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using PurpleFlowerCore.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PurpleFlowerCore
{
    public class DebugCommandInfo
    {
        private const string rootPath = "root";
        public string[] Path;
        public Action Command;
        private string _path;
        
        public DebugCommandInfo(string path, Action command)
        {
            path = rootPath + "/" + path;
            Path = path.Split('/');
            Command = command;
        }
        
    }
    public class DebugMenu : MonoBehaviour, IDragHandler
    {
        //todo:UI框架
        [SerializeField] private GameObject menu;
        [SerializeField] private Transform itemRoot;
        
        private readonly Dictionary<string,DebugCommandInfo> _menuCommands = new();
        private readonly List<DebugMenuItem> _items = new();
        [Inspectable][SerializeField]private List<string> _currentPath = new(){"root"};
        [Inspectable]private int _currentPathIndex = 0;
        private const string ItemUIPath = "PFCRes/DebugMenuItem";
        private HashSet<string> _itemNameBuffer = new();
        
        public void AddCommand(string commandName, Action command)
        {
            
            _menuCommands.Add(commandName, new DebugCommandInfo(commandName, command));
        }
        
        public void RemoveCommand(string commandName)
        {
            _menuCommands.Remove(commandName);
        }
        
        public void ExecuteCommand(string commandName)
        {
            if (_menuCommands.ContainsKey(commandName))
            {
                _menuCommands[commandName].Command?.Invoke();
            }
        }

        public void ClickItem(string itemName)
        {
            _currentPathIndex++;
            if(_menuCommands[itemName].Path.Length <= _currentPathIndex)
            {
                _currentPathIndex = _menuCommands[itemName].Path.Length;
                ExecuteCommand(itemName);
            }else
            {
                _currentPath.Add(_menuCommands[itemName].Path[_currentPathIndex]);
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
            if (_currentPathIndex == 0) return;
            _currentPathIndex--;
            _currentPath.RemoveAt(_currentPathIndex);
            Refresh();
        }
        
        private void ShowItem(string itemName)
        {
            var newItem = Instantiate(Resources.Load<DebugMenuItem>(ItemUIPath), itemRoot);
            _items.Add(newItem);
            newItem.Init(_menuCommands[itemName]);
        }

        private void ShowItems()
        {
            foreach (var command in _menuCommands)
            {
                bool isCurrentPath = true;
                for (int i = 0; i <= _currentPathIndex; i++)
                {
                    if (command.Value.Path[i] == _currentPath[i]) continue;
                    isCurrentPath = false;
                    break;
                }
                if(isCurrentPath)
                {
                    ShowItem(command.Key);
                    _itemNameBuffer.Add(command.Key);
                }
            }
        }

        public void Switch()
        {
            menu.SetActive(!menu.activeSelf);           
        }
        
        public void Refresh()
        {
            _itemNameBuffer.Clear();
            ClearItems();
            ShowItems();
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }
    }
}