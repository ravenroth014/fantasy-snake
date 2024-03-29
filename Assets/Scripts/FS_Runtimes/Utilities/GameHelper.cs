using FS_Runtimes.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FS_Runtimes.Utilities
{
    public static class GameHelper
    {
        #region Fields & Properties

        public static readonly string LoaderScene = "Loader";
        public static readonly string GameplayScene = "Gameplay";
        public static readonly string NavigatorScene = "Navigator";
        
        public static readonly string IdleState = "idle";
        public static readonly string JumpState = "jump";
        public static readonly string RunState = "run";
        
        public static readonly string MainTexture = "_MainTex";

        public static readonly string GameSettingKey = "GameSetting";
        
        #endregion

        #region Methods
        
        /// <summary>
        /// Call this method to get 2D direction by player action.
        /// </summary>
        /// <param name="playerAction"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Call this method to get character switch action by player action.
        /// </summary>
        /// <param name="playerAction"></param>
        /// <returns></returns>
        public static ECharacterSwitch GetCharacterSwitchAction(EPlayerAction playerAction)
        {
            switch (playerAction)
            {
                case EPlayerAction.RotateLeft:
                    return ECharacterSwitch.Left;
                case EPlayerAction.RotateRight:
                    return ECharacterSwitch.Right;
                default:
                    return ECharacterSwitch.None;
            }
        }
        
        /// <summary>
        /// Call this method to get player action by d-Pad gamepad.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Call this method to get player action by corner button gamepad.
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Call this method to get player action by keyboard.
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Call this method to get new game state instance.
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Call this method to calculate character stat with formula.
        /// </summary>
        /// <param name="baseStat"></param>
        /// <param name="level"></param>
        /// <param name="growthRate"></param>
        /// <returns></returns>
        public static int CalculateCharacterStat(int baseStat, int level, float growthRate)
        {
            return (int)(baseStat * (1 + (growthRate * (level - 1))));
        }
        
        #endregion
    }
}
