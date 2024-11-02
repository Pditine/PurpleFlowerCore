using PurpleFlowerCore.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace PurpleFlowerCore.Editor.ItemMenu
{
    public static class Create
    {
        private static PrefabRef _prefabRef;
        public static PrefabRef PrefabRef
        {
            get
            {
                if (_prefabRef == null)
                    _prefabRef = SOUtility.GetSOByType<PrefabRef>(true);
                return _prefabRef;
            }
        }
        [MenuItem("GameObject/PFC/PropertyBar")]
        public static void CreatePropertyBar()
        {
            CreatePrefab(PrefabRef.PropertyBar);
        }
        
        private static void CreatePrefab(MonoBehaviour prefab)
        {
            var obj = GameObject.Instantiate(prefab);
            obj.name = prefab.name;
        }
    }
}