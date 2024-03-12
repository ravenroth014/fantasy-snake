using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FS_Runtimes.UI_Extension
{
    [RequireComponent(typeof(Button))]
    public class SelectedButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        #region Fields & Properties

        [SerializeField, Tooltip("Button")] private Button _button;
        [SerializeField, Tooltip("Button's Text")] private TextMeshProUGUI _text;
        [SerializeField, Tooltip("Highlight Selected UI Object")] private List<GameObject> _highlightItemList;
        
        #endregion

        #region Methods

        #region Set Methods

        /// <summary>
        /// Call this method to set button's callback.
        /// </summary>
        /// <param name="onClick"></param>
        public void SetCallback(Action onClick = null)
        {
            if (_button is null) return;
            
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() =>
            {
                onClick?.Invoke();
                SetHighlightActive(false);
            });
        }

        /// <summary>
        /// Call this method to set button's text value.
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            if (_text is null) return;

            _text.text = text;
        }

        /// <summary>
        /// Call this method to set external highlight object state.
        /// </summary>
        /// <param name="state"></param>
        private void SetHighlightActive(bool state)
        {
            if (_highlightItemList is null or {Count: 0})
                return;
            
            _highlightItemList.ForEach(item => item.SetActive(state));
        }
        
        #endregion

        #region Unity UI Event Methods
        
        public void OnSelect(BaseEventData eventData)
        {
            SetHighlightActive(true);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            SetHighlightActive(false);
        }
        
        #endregion
        
        #endregion
    }
}
