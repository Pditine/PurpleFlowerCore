using UnityEditor.Graphs;
using UnityEngine;

namespace PurpleFlowerCore
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
    }
    public struct LogData
    {
        public LogLevel Level;
        public string Channel;
        public string Content;
        public string Time;
        public Color Color;
    }
    public static class PFCLog
    {
        public static void Print(LogLevel level, object content)
        {
            Print(level, Color.white,null, content);
        }
        
        public static void Print(LogLevel level, string channel, object content)
        {
            Print(level, Color.white, channel, content);
        }

        public static void Print(LogLevel level,Color color, string channel, object content)
        {
            var prefixColor = DebugSystem.GetLogLevelColor(level);
            var prefix = level.ToString();
            string fullContent;
            if (channel == null)
            {
                fullContent = $"<color=#{ColorUtility.ToHtmlStringRGB(prefixColor)}>[PFC_{prefix}]</color>" +
                          $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{content}</color>";
            }
            else
            {
                fullContent = $"<color=#{ColorUtility.ToHtmlStringRGB(prefixColor)}>[PFC_{prefix}][{channel}]</color>" +
                          $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{content}</color>";
            }
            switch(level)
            {
                case LogLevel.Debug:
                    UnityEngine.Debug.Log(fullContent);
                    break;
                case LogLevel.Info:
                    UnityEngine.Debug.Log(fullContent);
                    break;
                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarning(fullContent);
                    break;
                case LogLevel.Error:
                    UnityEngine.Debug.LogError(fullContent);
                    break;
                default:
                    UnityEngine.Debug.Log(fullContent);
                    break;
            }
#if PFC_DEBUGMENU
            var logData = new LogData
            {
                Level = level,
                Channel = channel,
                Content = content.ToString(),
                Time = System.DateTime.Now.ToString("HH:mm:ss"),
                Color = color
            };
            DebugSystem.Log(logData);
#endif
        }
        
        public static void Debug(string content)
        {
#if !NOT_PFC_LOG_DEBUG
            Print(LogLevel.Debug, content.ToString());
#endif
        }
//         public static void Debug(object content,Color color)
//         {
// #if !NOT_PFC_LOG_DEBUG
//             Print(LogLevel.Debug, color, content.ToString());
// #endif
//         }
        
        public static void Debug(string channel, object content)
        {
#if !NOT_PFC_LOG_DEBUG
            Print(LogLevel.Debug, Color.white,channel, content);
#endif
        }
        
        public static void Debug(string channel, params object[] content)
        {
#if !NOT_PFC_LOG_DEBUG
            string contentStr = string.Join(' ', content);
            Print(LogLevel.Debug, Color.white,channel, contentStr);
#endif
        }
        public static void Debug(string channel, object content,Color color)
        {
#if !NOT_PFC_LOG_DEBUG
            Print(LogLevel.Debug, color, channel, content);
#endif
        }

        public static void Info(string content)
        {
#if !NOT_PFC_LOG_INFO
            Print(LogLevel.Info, content.ToString());
#endif
        }
//         public static void Info(object content,Color color)
//         {
// #if !NOT_PFC_LOG_INFO
//             Print(LogLevel.Info, color, content.ToString());
// #endif
//         }
        
        public static void Info(string channel, object content)
        {
#if !NOT_PFC_LOG_INFO
            Print(LogLevel.Info, channel, content);
#endif
        }
        
        public static void Info(string channel, params object[] content)
        {
#if !NOT_PFC_LOG_INFO
            string contentStr = string.Join(' ', content);
            Print(LogLevel.Info, Color.white, channel, contentStr);
#endif
        }
        
        public static void Info(string channel, object content,Color color)
        {
#if !NOT_PFC_LOG_INFO
            Print(LogLevel.Info, color, channel, content);
#endif
        }
        
        public static void Warning(string content)
        {
#if !NOT_PFC_LOG_WARNING
            Print(LogLevel.Warning, content.ToString());
#endif
        }
        
//         public static void Warning(object content,Color color)
//         {
// #if !NOT_PFC_LOG_WARNING
//             Print(LogLevel.Warning, color, content.ToString());
// #endif
//         }
        
        public static void Warning(string channel, object content)
        {
#if !NOT_PFC_LOG_WARNING
            Print(LogLevel.Warning, channel, content);
#endif
        }

        public static void Warning(string channel, params object[] content)
        {
#if !NOT_PFC_LOG_WARNING
            string contentStr = string.Join(' ', content);
            Print(LogLevel.Warning, Color.white, channel, contentStr);
#endif
        }

        public static void Warning(string channel, object content,Color color)
        {
#if !NOT_PFC_LOG_WARNING
            Print(LogLevel.Warning, color, channel, content);
#endif
        }
        
        public static void Error(string content)
        {
#if !NOT_PFC_LOG_ERROR
            Print(LogLevel.Error, content.ToString());
#endif
        }
        
//         public static void Error(object content,Color color)
//         {
// #if !NOT_PFC_LOG_ERROR
//             Print(LogLevel.Error, color, content.ToString()); 
// #endif
//         }
        public static void Error(string channel, object content)
        {
#if !NOT_PFC_LOG_ERROR
            Print(LogLevel.Error, channel, content);
#endif
        }
        
        public static void Error(string channel, params object[] content)
        {
#if !NOT_PFC_LOG_ERROR
            string contentStr = string.Join(' ', content);
            Print(LogLevel.Error, Color.white, channel, contentStr);
#endif
        }
        
        public static void Error(string channel, object content,Color color)
        {
#if !NOT_PFC_LOG_ERROR
            Print(LogLevel.Error, color, channel, content);
#endif
        }
    }
}