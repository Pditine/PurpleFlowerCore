using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PurpleFlowerCore
{
    public class LogPanel : MonoBehaviour
    {
        private const string LogInfoPath = "PFCRes/LogInfo";
        [SerializeField]private ScrollRect scrollRect;
        private List<LogInfo> _logInfos = new();
        
        public void Print(string info)
        {
            LogInfo logInfo = Instantiate(Resources.Load<LogInfo>(LogInfoPath), scrollRect.content);
            logInfo.Content = info;
            scrollRect.normalizedPosition = new Vector2(0, 0);
        }
    }
}