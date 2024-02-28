using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PurpleFlowerCore.Utility
{
    public static class DelayUtility
    {
        public static void Delay(float time,UnityAction action)
        {
            MonoSystem.Start_Coroutine(DoDelay(time, action));
        }

        private static IEnumerator DoDelay(float time,UnityAction action)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }
    }
}