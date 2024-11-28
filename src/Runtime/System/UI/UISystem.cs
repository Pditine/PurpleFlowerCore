using System.Collections.Generic;
using UnityEngine;

namespace PurpleFlowerCore
{
    public static class UISystem
    {
        private static readonly Dictionary<string, UINode> UIs = new();
        
        public static bool RegisterUI(string name, UINode ui)
        {
            if (UIs.ContainsKey(name))
            {
                return false;
            }
            UIs.Add(name, ui);
            return true;
        }
        
        public static bool UnRegisterUI(string name)
        {
            if (UIs.ContainsKey(name))
            {
                UIs.Remove(name);
                return true;
            }
            return false;
        }
        
        public static UINode GetUI(string name)
        {
            return UIs.TryGetValue(name, out var ui) ? ui : null;
        }
        
        public static T GetUI<T>(string name) where T : UINode
        {
            return UIs.TryGetValue(name, out var ui) ? ui as T : null;
        }
        
        public static bool ShowUI(string name)
        {
            if (UIs.TryGetValue(name, out var ui))
            {
                ui.Show();
                return true;
            }
            return false;
        }
        
        public static bool HideUI(string name)
        {
            if (UIs.TryGetValue(name, out var ui))
            {
                ui.Hide();
                return true;
            }
            return false;
        }

        public static bool SwitchUI(string name, bool show)
        {
            if (UIs.TryGetValue(name, out var ui))
            {
                ui.Switch(show);
                return true;
            }
            return false;
        }
    }
}