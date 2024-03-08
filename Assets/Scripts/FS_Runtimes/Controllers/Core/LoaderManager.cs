using FS_Runtimes.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FS_Runtimes.Controllers.Core
{
    public class LoaderManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Initialize Setting")] 
        [SerializeField, Tooltip("Target Framerate")] private int _targetFrameRate = 60;
        
        #endregion

        #region Methods

        #region Unity Event Methods
        
        private void Awake()
        {
            Init();
            StartProcess();
        }
        
        #endregion

        #region Init Methods

        private void Init()
        {
            Application.targetFrameRate = _targetFrameRate;
        }
        
        private void StartProcess()
        {
            NavigateToScene(SceneHelper.GameplayScene, false);
            NavigateToScene(SceneHelper.NavigatorScene, true);
            SceneManager.UnloadSceneAsync(SceneHelper.LoaderScene);
        }
        
        #endregion
        
        #region Load Scene Methods
        
        private void NavigateToScene(string sceneName, bool isAdditive)
        {
            if (isAdditive == false)
                SceneManager.LoadSceneAsync(sceneName);
            else
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        
        #endregion
        
        #endregion
    }
}
