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
        
        public static void Lerp(Vector3 target, Transform transform, float speed, UnityAction callBack = null)
        {
            if (transformBuffer.Contains(transform)) return;
            AddBuffer(transform);
            MonoSystem.Start_Coroutine(DoLerp(target, transform, speed, callBack));
        }

        private static IEnumerator DoLerp(Vector3 target, Transform transform, float speed, UnityAction callBack)
        {
            while (Mathf.Abs(transform.localScale.x-target.x)>0.01f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, target, 1/speed);
                yield return new WaitForSeconds(0.01f);
            }
            transform.localScale = target;
            RemoveBuffer(transform);
            callBack?.Invoke();
            
        }
        
        public static void MoveTowards(Vector3 target, Transform transform, float speed, UnityAction callBack = null)
        {
            if (transformBuffer.Contains(transform)) return;
            AddBuffer(transform);
            MonoSystem.Start_Coroutine(DoMoveTowards(target, transform, speed, callBack));
        }
        
        private static IEnumerator DoMoveTowards(Vector3 target, Transform transform, float speed, UnityAction callBack)
        {
            while (Vector3.Distance(transform.localScale, target)>0.01f)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, target, speed);
                yield return new WaitForSeconds(0.01f);
            }
            transform.localScale = target;
            RemoveBuffer(transform);
            callBack?.Invoke();
        }
    }
}