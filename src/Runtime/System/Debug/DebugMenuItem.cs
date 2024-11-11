using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PurpleFlowerCore
{
    public class DebugMenuItem : MonoBehaviour
    {
        [SerializeField] private Text commandName;
        [SerializeField] private Button commandButton;
        
        public void Init(DebugCommandInfo commandInfo)
        {
            commandName.text = commandInfo.Path.Last();
            // commandButton.onClick.AddListener();    
        }
    }
}