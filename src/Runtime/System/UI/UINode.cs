using System;
using UnityEngine;

namespace PurpleFlowerCore
{
    public abstract class UINode: MonoBehaviour
    {
#if UNITY_EDITOR
        [HideInInspector] public Color TagColor = Color.white;
        //[HideInInspector] public String NodeName = "UINode";
#endif
        private void Awake()
        {
            InitEvent();
        }

        /// <summary>
        /// 禁止手动复写
        /// </summary>
        protected virtual void InitEvent()
        {
            
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}