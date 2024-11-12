using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace PurpleFlowerCore
{
    //todo: 显示调用栈
    public class LogInfo : MonoBehaviour
    {
        [SerializeField] private Text text;

        private LogData _data;

        public LogData Data
        {
            set
            {
                _data = value;
                SetText();
            }
            get => _data;
        }
        // private string _channel;
        // public string Channel => _channel;
        // private LogLevel _level;
        // public LogLevel Level => _level;
        // private string _content;
        // public string Content => _content;
        // private string _time;
        // public string Time => _time;

        public void Init(LogData data)
        {
            // _channel = data.Content;
            // _channel = data.Channel;
            // _level = data.Level;
            // _time = data.Time;
            _data = data;
            SetText();
        }
        
        private void SetText()
        {
            var prefixColor = DebugSystem.GetLogLevelColor(_data.Level);
            var prefix = $"<color=#{ColorUtility.ToHtmlStringRGB(prefixColor)}>[{_data.Level}]</color>";
            var channel = _data.Channel == null ? "" : $"[{_data.Channel}]";
            var time = _data.Time == null ? "" : $"[{_data.Time}]";
            var content = $"<color=#{ColorUtility.ToHtmlStringRGB(_data.Color)}>{_data.Content}</color>";

            var sb = new StringBuilder();
            sb.Append(prefix);
            sb.Append(time);
            sb.Append(channel);
            sb.Append(content);

            text.text = sb.ToString();
            text.rectTransform.sizeDelta = new Vector2(text.rectTransform.sizeDelta.x, text.preferredHeight);
        }

    }
}