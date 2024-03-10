using System;
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

        public static Vector2 Get2DDirection(EDirection direction)
        {
            switch (direction)
            {
                case EDirection.Up:
                    return Vector2.up;
                case EDirection.Down:
                    return Vector2.down;
                case EDirection.Left:
                    return Vector2.left;
                case EDirection.Right:
                    return Vector2.right;
                case EDirection.None:
                default:
                    return Vector2.zero;
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

        #region Gameplay Direction Methods

        public static Vector2 GetVector2Direction(EPlayerAction playerAction)
        {
            switch (playerAction)
            {
                case EPlayerAction.Up:
                    return Vector2.up;
                case EPlayerAction.Right:
                    return Vector2.right;
                case EPlayerAction.Down:
                    return Vector2.down;
                case EPlayerAction.Left:
                    return Vector2.left;
                default:
                    return Vector2.zero;
            }
        }

        #endregion

        #region Player Action Methods
        
        public static EPlayerAction GetPlayerAction(int minValue, int maxValue)
        {
            switch (minValue)
            {
                case 7 when maxValue == 1:
                    return EPlayerAction.Up;
                case 1 when maxValue == 3:
                    return EPlayerAction.Right;
                case 3 when maxValue == 5:
                    return EPlayerAction.Down;
                case 5 when maxValue == 7:
                    return EPlayerAction.Left;
                default:
                    return EPlayerAction.None;
            }
        }

        public static EPlayerAction GetPlayerAction(string buttonName)
        {
            switch (buttonName)
            {
                case "leftShoulder":
                    return EPlayerAction.RotateLeft;
                case "rightShoulder":
                    return EPlayerAction.RotateRight;
                default:
                    return EPlayerAction.None;
            }
        }

        public static EPlayerAction GetPlayerAction(Key keyCode)
        {
            switch (keyCode)
            {
                case Key.W:
                    return EPlayerAction.Up;
                case Key.D:
                    return EPlayerAction.Right;
                case Key.S:
                    return EPlayerAction.Down;
                case Key.A:
                    return EPlayerAction.Left;
                case Key.Q:
                    return EPlayerAction.RotateLeft;
                case Key.E:
                    return EPlayerAction.RotateRight;
                default:
                    return EPlayerAction.None;
            }
        }
        
        #endregion

        #region Game State Methods
        
        public static GameState GetGameState(EGameState gameState)
        {
            switch (gameState)
            {
                case EGameState.GameMenu:
                    return new GameMenuState();
                case EGameState.GamePlay:
                    return new GameplayState();
                case EGameState.GameOver:
                    return new GameOverState();
                case EGameState.GameError:
                    return new GameErrorState();
                default:
                    return null;
            }
        }
        
        #endregion
    }
}
