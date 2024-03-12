using TMPro;
using UnityEngine;

namespace FS_Runtimes.Controllers.UI
{
    public class LoadingUIController : AbstractUIController
    {
        #region Fields & Properties

        [Header("Text UI")]
        [SerializeField, Tooltip("Loading Detail Text")] private TextMeshProUGUI _loadingDetailText;

        #endregion

        #region Methods

        /// <summary>
        /// Call this method to set loading detail.
        /// </summary>
        /// <param name="detail"></param>
        public void SetDetailText(string detail)
        {
            if (_loadingDetailText is not null)
                _loadingDetailText.text = detail;
        }

        #endregion
    }
}
