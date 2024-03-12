using UnityEngine;

namespace FS_Runtimes.Utilities.Setting
{
    [CreateAssetMenu(fileName = "CharacterGameObjectSetting", menuName = "Settings/Character GameObject Setting")]
    public class CharacterGameObjectSetting : ScriptableObject
    {
        #region Fields & Properties
        
        [Header("Character Setting")] 
        [SerializeField, Tooltip("List of Texture")] private Texture2D[] _textureList;
        [SerializeField, Tooltip("Character speed")] private float _speed = 5f;
        [SerializeField, Tooltip("Character position threshold")] private float _threshold = 0.01f;

        private int TextureCount => _textureList is null or { Length: <= 0 } ? 0 : _textureList.Length;
        public float Speed => _speed;
        public float Threshold => _threshold;
        
        #endregion

        #region Methods
        
        /// <summary>
        /// Call this method to get random texture for character.
        /// </summary>
        /// <returns></returns>
        public Texture2D GetRandomTexture()
        {
            int randomIndex = Random.Range(0, TextureCount);
            return _textureList[randomIndex];
        }
        
        #endregion
    }
}
