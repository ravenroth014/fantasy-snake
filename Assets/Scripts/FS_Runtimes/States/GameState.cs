using System;
using System.Collections;
using UnityEngine;

namespace FS_Runtimes.States
{
    public abstract class GameState 
    {
        #region Methods

        #region Abstract Methods

        public abstract void OnEnter();

        public abstract void OnExit();

        #endregion

        #region Utility Methods

        protected IEnumerator DelayTime(int timeInSecond, Action onComplete = null)
        {
            if (timeInSecond <= 0)
                yield return null;
            
            yield return new WaitForSeconds(timeInSecond);
            
            onComplete?.Invoke();
        }

        #endregion
        
        #endregion
    }
}
