using System.Threading.Tasks;
using UnityEngine;

namespace FS_Runtimes.Controllers.Core
{
    public class GameplayManager : MonoBehaviour
    {
        #region Fields & Properties
        
        public static GameplayManager Instance => _instance;
        private static GameplayManager _instance;
        
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
