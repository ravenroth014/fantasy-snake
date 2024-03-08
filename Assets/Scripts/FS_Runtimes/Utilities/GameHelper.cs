using System;
using UnityEngine;

namespace FS_Runtimes.Utilities
{
    public static class SceneHelper
    {
        public static readonly string LoaderScene = "Loader";
        public static readonly string GameplayScene = "Gameplay";
        public static readonly string NavigatorScene = "Navigator";
    }

    public static class CharacterAnimationHelper
    {
        public static readonly string IdleState = "idle";
        public static readonly string JumpState = "jump";
        public static readonly string RunState = "run";
    }

    public static class MaterialPropertyHelper
    {
        public static readonly string MainTexture = "_MainTex";
    }

    public static class DirectionHelper
    {
        public static Vector3 GetDirection(EDirection direction)
        {
            switch (direction)
            {
                case EDirection.Up:
                    return Vector3.forward;
                case EDirection.Down:
                    return Vector3.back;
                case EDirection.Left:
                    return Vector3.left;
                case EDirection.Right:
                    return Vector3.right;
                default:
                    return Vector3.zero;
            }
        }
    }

    public static class GridHelper
    {
        public static Vector2 GetGridVector(Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }
    }
}
