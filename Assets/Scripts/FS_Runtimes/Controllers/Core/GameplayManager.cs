using FS_Runtimes.Controllers.Player;
using UnityEngine;

namespace FS_Runtimes.Controllers.Core
{
    public class GameplayManager : MonoBehaviour
    {
        #region Fields & Properties
        
        public static GameplayManager Instance => _instance;
        private static GameplayManager _instance;

        public CharacterModule Test;
        
        #endregion

        #region Methods

        #region Unity Event Methods
        
        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            if (Test is not null)
                Test.Init();
        }

        #endregion
        
        #endregion
    }
}
