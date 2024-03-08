using UnityEngine;

namespace FS_Runtimes.Utilities
{
    public static class Extensions
    {
        public static bool HasComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() != null;
        }
    }
}
