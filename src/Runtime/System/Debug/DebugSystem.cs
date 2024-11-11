using UnityEngine;
using PurpleFlowerCore.Base;

namespace PurpleFlowerCore
{
    public static class DebugSystem
    {
        private static DebugMenu _debugMenu;
        private static DebugMenu DebugMenu
        {
            get
            {
                if (_debugMenu is not null) return _debugMenu;
                DebugMenu root = GameObject.Instantiate(Resources.Load<DebugMenu>("PFCRes/DebugMenu"));
                root.transform.parent = PFCManager.Instance.transform;
                _debugMenu = root;
                return _debugMenu;
            }
        }

        public static void AddCommand(string commandName, System.Action command)
        {
            
        }
        
        public static void ClickItem(string itemName)
        {
            DebugMenu.ClickItem(itemName);
        }
    }
}