using UnityEngine;

namespace Menu
{
    public static class TransformExtensions
    {
        public static void SetParent(this Transform child, Transform parent, Vector3? position = null, Quaternion? rotation = null, Vector3? scale = null)
        {
            child.parent = parent;
            child.localPosition = position ?? Vector3.zero;
            child.localRotation = rotation ?? Quaternion.identity;
            child.localScale = scale ?? Vector3.one;
        }
    }
}