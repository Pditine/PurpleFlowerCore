using UnityEngine;

namespace PurpleFlowerCore
{

    public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        public static T Instance;
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
        }
    }

    public abstract class DdolSingletonMono<T> : MonoBehaviour where T : DdolSingletonMono<T>
    {
        public static T Instance;
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}