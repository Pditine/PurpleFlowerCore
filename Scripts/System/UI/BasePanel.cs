using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PurpleFlowerCore.UI
{
    public class BasePanel : MonoBehaviour
    {
        public Dictionary<string, UIBehaviour> controlDic = new();
    }
}