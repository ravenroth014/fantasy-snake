using System.Collections.Generic;
using FS_Runtimes.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FS_Runtimes.Utilities
{
    public static class GameHelper
    {
        public static readonly string LoaderScene = "Loader";
        public static readonly string GameplayScene = "Gameplay";
        public static readonly string NavigatorScene = "Navigator";
        
        public static readonly string IdleState = "idle";
        public static readonly string JumpState = "jump";
        public static readonly string RunState = "run";
        
        public static readonly string MainTexture = "_MainTex";

        private static readonly Dictionary<EDirection, List<EDirection>> _availableDirectionDict = new();
        
        public static Vector3 GetWorldSpaceDirection(EDirection direction)
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

        public static EDirection GetDirection(Vector3 vector3)
        {
            if (vector3.normalized == Vector3.forward)
                return EDirection.Up;
            if (vector3.normalized == Vector3.back)
                return EDirection.Down;
            if (vector3.normalized == Vector3.left)
                return EDirection.Left;
            if (vector3.normalized == Vector3.right)
                return EDirection.Right;
            return EDirection.None;
        }
        
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
        
        public static Vector2 GetGridVector(Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }
        
        public static GameState GetGameState(EGameState gameState)
        {
            switch (gameState)
            {
                case EGameState.GameMenu:
                    return new GameMenuState();
                case EGameState.GamePrepare:
                    return new GamePrepareState();
                case EGameState.GamePlay:
                    return new GameplayState();
                case EGameState.GameOver:
                    return new GameOverState();
                default:
                    return null;
            }
        }
    }
}
