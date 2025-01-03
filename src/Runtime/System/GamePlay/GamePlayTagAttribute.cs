using UnityEngine;

namespace GP.GameplayKit
{
    // 用于将string字段标记为GamePlayTag
    public class GamePlayTagAttribute : PropertyAttribute
    {
        public string Catalogue { get; }
        public GamePlayTagAttribute(string catalogue)
        {
            Catalogue = catalogue;
        }
        
        public GamePlayTagAttribute()
        {
            Catalogue = null;
        }
    }
}