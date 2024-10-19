using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PurpleFlowerCore.Utility
{
    public static class ScaleUtility
    {
        public static HashSet<Transform> transformBuffer = new();

        public static void AddBuffer(Transform transform)
        {
            transformBuffer.Add(transform);
        }

        public static void RemoveBuffer(Transform transform)
        {
            if(!transformBuffer.Contains(transform)) return;
            transformBuffer.Remove(transform);
        }
        
        
        public static void LerpTo(Vector3 target, Transform transform, float speed, UnityAction callBack = null)
        {
            AddBuffer(transform);
            MonoSystem.Start_Coroutine(DoLerpTo(target, transform, speed, callBack));
        }

        private static IEnumerator DoLerpTo(Vector3 target, Transform transform, float speed, UnityAction callBack)
        {
            while (!transform.localScale.x.Equals(target.x))
            {
                transform.localScale = Vector3.Lerp(transform.localScale, target, 1/speed);
                yield return new WaitForSeconds(0.01f);
            }
            RemoveBuffer(transform);
            callBack?.Invoke();
        }
    }
}