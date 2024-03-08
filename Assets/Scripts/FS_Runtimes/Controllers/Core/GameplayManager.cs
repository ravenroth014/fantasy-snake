using FS_Runtimes.Controllers.Level;
using FS_Runtimes.Controllers.Player;
using FS_Runtimes.Controllers.Pooling;
using UnityEngine;

namespace FS_Runtimes.Controllers.Core
{
    public class GameplayManager : MonoBehaviour
    {
        #region Fields & Properties

        public CharacterPooling HeroPooling => _heroPooling;
        [SerializeField, Tooltip("Hero Pooling")]
        private CharacterPooling _heroPooling;

        public CharacterPooling EnemyPooling => _enemyPooling;
        [SerializeField, Tooltip("Enemy Pooling")]
        private CharacterPooling _enemyPooling;

        public LevelManager LevelManager => _levelManager;
        [SerializeField, Tooltip("Level Manager")]
        private LevelManager _levelManager;

        public PlayerManager PlayerManager => _playerManager;
        [SerializeField, Tooltip("Player Manager")]
        private PlayerManager _playerManager;
        
        public static GameplayManager Instance => _instance;
        private static GameplayManager _instance;

        // public CharacterModule Test;
        
        #endregion

        #region Methods

        #region Unity Event Methods
        
        private void Awake()
        {
            _instance = this;
        }

        #endregion

        #endregion
    }
}
