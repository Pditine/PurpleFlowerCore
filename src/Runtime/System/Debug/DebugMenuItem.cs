using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PurpleFlowerCore
{
    public class DebugMenuItem : MonoBehaviour
    {
        [SerializeField] private Text commandName;
        [SerializeField] private Button commandButton;
        
        public void Init(TreeNode<Action> commandNode, DebugMenu debugMenu)
        {
            commandButton.onClick.RemoveAllListeners();
            var text = commandNode.IsLeaf? commandNode.name: commandNode.name + ">" ;
            commandName.text = text;
            commandButton.onClick.AddListener(() => debugMenu.ClickItem(commandNode));
        }
    }
}