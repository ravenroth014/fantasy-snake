using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FS_Runtimes.Controllers.UI
{
    public class GameOverUIController : AbstractUIController
    {
        #region Fields & Properties

        [Header("Text UI")] 
        [SerializeField, Tooltip("Score Text")] private TextMeshProUGUI _scoreText;

        [Header("Button UI")]
        [SerializeField, Tooltip("Retry Button")] private Button _retryButton;
        [SerializeField, Tooltip("Main Menu Button")] private Button _mainMenuButton;

        #endregion

        #region Methods

        public void Init()
        {
            if (_retryButton is not null)
            {
                _retryButton.onClick.RemoveAllListeners();
                _retryButton.onClick.AddListener(OnClickRetryButton);
            }

            if (_mainMenuButton is not null)
            {
                _mainMenuButton.onClick.RemoveAllListeners();
                _mainMenuButton.onClick.AddListener(OnClickMainMenuButton);
            }
        }

        public void SetScore(int score)
        {
            string scoreString = score.ToString("D");

            if (_scoreText is not null)
                _scoreText.text = $"Score : {scoreString}";
        }

        private void OnClickRetryButton()
        {
            GameManager.Instance.ChangeState(EGameState.GamePlay);
        }

        private void OnClickMainMenuButton()
        {
            GameManager.Instance.ChangeState(EGameState.GameMenu);
        }

        #endregion
    }
}
