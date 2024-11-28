﻿using System;
using UnityEngine;

namespace PurpleFlowerCore
{
    public abstract class UINode: MonoBehaviour
    {
#if UNITY_EDITOR
        [HideInInspector] public Color TagColor = Color.white;
        [HideInInspector] public String NodeName = "";
#endif
        private void Awake()
        {
            InitEvent();
        }

        /// <summary>
        /// Do not call or override this method manually
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