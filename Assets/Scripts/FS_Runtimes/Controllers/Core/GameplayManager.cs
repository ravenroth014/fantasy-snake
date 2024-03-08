using FS_Runtimes.Controllers.Level;
using FS_Runtimes.Controllers.Player;
using FS_Runtimes.Controllers.Pooling;
using FS_Runtimes.Utilities;
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

        private void Start()
        {
            // if (Test is not null)
            //     Test.Init();
        }

        private void Update()
        {
            TestCharacterRotation();
        }

        #endregion

        private void TestCharacterRotation()
        {
            // if (Input.GetKeyDown(KeyCode.W))
            //     Test.SetCharacterPosition(EDirection.Up);
            // else if (Input.GetKeyDown(KeyCode.A))
            //     Test.SetCharacterPosition(EDirection.Left);
            // else if (Input.GetKeyDown(KeyCode.D))
            //     Test.SetCharacterPosition(EDirection.Right);
            // else if (Input.GetKeyDown(KeyCode.S))
            //     Test.SetCharacterPosition(EDirection.Down);
        }
        
        #endregion
    }
}
