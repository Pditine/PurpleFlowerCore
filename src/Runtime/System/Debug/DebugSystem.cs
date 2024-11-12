using System;
using UnityEngine;
using PurpleFlowerCore.Base;

namespace PurpleFlowerCore
{
    /// <summary>
    /// 在游戏运行时调试系统,在设置中打开"使用运行时Debug菜单"
    /// </summary>
    public static class DebugSystem
    {
        private static bool _isDebugMenuOpen;
        private static DebugMenu _debugMenu;
        private static DebugMenu DebugMenu
        {
            get
            {
                if (_debugMenu is not null) return _debugMenu;
                DebugMenu root = GameObject.Instantiate(Resources.Load<DebugMenu>("PFCRes/DebugMenu"), PFCManager.Canvas.transform);
                _debugMenu = root;
                return _debugMenu;
            }
        }
#if PRC_DEBUGMENU
        
        [RuntimeInitializeOnLoadMethod]
        private static void OnGameStart()
        {
            MonoSystem.AddUpdateListener(DebugMenuOpen);
        }

        private static void DebugMenuOpen()
        {
            if (!Input.GetKeyDown(KeyCode.BackQuote)) return;
            _isDebugMenuOpen = !_isDebugMenuOpen;
            DebugMenu.gameObject.SetActive(_isDebugMenuOpen);
            DebugMenu.transform.localPosition = Vector3.zero;
            DebugMenu.Switch(_isDebugMenuOpen);
        }
#endif

        public static void AddCommand(string commandName, Action command)
        {
            DebugMenu.AddCommand(commandName, command);
        }
        
        public static void Log(PFCLog.LogLevel level,string info)
        {
            DebugMenu.Print(info);
        }
        
        // public static void ClickItem(TreeNode<Action> commandNode)
        // {
        //     DebugMenu.ClickItem(commandNode);
        // }
    }
}