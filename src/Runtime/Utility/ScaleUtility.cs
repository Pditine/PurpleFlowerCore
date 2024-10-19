using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PurpleFlowerCore.Utility
{
    public class ScaleUtility
    {
        public static void LerpTo(Vector3 target, Transform transform, float speed, UnityAction callBack = null)
        {
            MonoSystem.Start_Coroutine(DoLerpTo(target, transform, speed, callBack));
        }

        private static IEnumerator DoLerpTo(Vector3 target, Transform transform, float speed, UnityAction callBack)
        {
            while (!transform.localScale.x.Equals(target.x))
            {
                transform.localScale = Vector3.Lerp(transform.localScale, target, 1/speed);
                yield return new WaitForSeconds(0.01f);
            }
            callBack?.Invoke();
        }
    }
}