using System;
using FS_Runtimes.States;
using UnityEngine;

namespace FS_Runtimes.Controllers.Core
{
    public class GameplayManager : MonoBehaviour
    {
        #region Fields & Properties

        private GameState _currentState;
        private Action _onAttackEnemyAction;
        private Action _onRecruitEnlistAction;
        private Action _onHeroIsDeadAction;
        private Action _onGameOver;
        
        #endregion

        #region Methods

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
