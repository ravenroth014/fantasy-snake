using System;
using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Level;
using FS_Runtimes.States;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.Controllers.Core
{
    public class GameplayManager : MonoBehaviour
    {
        #region Fields & Properties

        private Action _onAttackEnemyAction;
        private Action _onRecruitEnlistAction;
        private Action _onHeroIsDeadAction;
        private Action _onGameOver;

        private bool _isGameStart;
        private EDirection _currentDirection;

        private LevelManager _levelManager;
        private CharactersManager _charactersManager;
        
        #endregion

        #region Methods

        public void Init()
        {
            _levelManager = GameManager.Instance.LevelManager;
            _charactersManager = GameManager.Instance.CharactersManager;
        }
        
        public void StartGame()
        {
            _isGameStart = true;
            _currentDirection = EDirection.None;
        }

        public void OnMoveAction(EDirection directionAction)
        {
            if (_isGameStart == false) return;
            
            
        }
        
        public void SetOnAttackEnemyCallback(Action callback = null)
        {
            _onAttackEnemyAction = callback;
        }

        public void SetOnRecruitEnlistCallback(Action callback = null)
        {
            _onRecruitEnlistAction = callback;
        }

        public void SetOnHeroIsDeadCallback(Action callback = null)
        {
            _onHeroIsDeadAction = callback;
        }

        public void SetOnGameOverCallback(Action callback = null)
        {
            _onGameOver = callback;
        }

        #endregion
    }
}
