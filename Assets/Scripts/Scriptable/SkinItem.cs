using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(menuName = "Game/Skin")]
    public class SkinItem : ScriptableObject
    {
        public Sprite sprite;
        public bool unlocked;
        [Min(1)] public int cost;
    }
}
