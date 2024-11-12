using UnityEngine;
using UnityEngine.UI;

namespace PurpleFlowerCore
{
    //todo: 显示调用栈
    public class LogInfo : MonoBehaviour
    {
        [SerializeField] private Text text;
        private string _content;

        public string Content
        {
            set
            {
                _content = value;
                text.text = _content;
            }
            get => _content;
        }
    }
}