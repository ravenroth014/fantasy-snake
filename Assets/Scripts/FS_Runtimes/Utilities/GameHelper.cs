using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

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
                case EDirection.None:
                default:
                    return Vector3.zero;
            }
        }
    }

    public static class KeyboardHelper
    {
        public static EDirection GetDirection(Key keyCode)
        {
            switch (keyCode)
            {
                case Key.A:
                    return EDirection.Left;
                case Key.W:
                    return EDirection.Up;
                case Key.S:
                    return EDirection.Down;
                case Key.D:
                    return EDirection.Right;
                default:
                    return EDirection.None;
            }
        }
    }

    public static class GamePadHelper
    {
        public static EDirection GetDirection(int minValue, int maxValue)
        {
            switch (minValue)
            {
                case 7 when maxValue == 1:
                    return EDirection.Up;
                case 1 when maxValue == 3:
                    return EDirection.Right;
                case 3 when maxValue == 5:
                    return EDirection.Down;
                case 5 when maxValue == 7:
                    return EDirection.Left;
                default:
                    return EDirection.None;
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
