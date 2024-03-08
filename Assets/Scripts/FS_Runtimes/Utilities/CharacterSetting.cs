using UnityEngine;

namespace FS_Runtimes.Utilities
{
    [CreateAssetMenu(fileName = "CharacterSetting", menuName = "Settings/Character Setting")]
    public class CharacterSetting : ScriptableObject
    {
        [Header("Character Setting")] 
        [SerializeField, Tooltip("List of Texture")] private Texture2D[] _textureList;

        public int TextureCount => _textureList is null or { Length: <= 0 } ? 0 : _textureList.Length;

        public Texture2D GetTextureByIndex(int index)
        {
            if (index < 0 || index >= TextureCount)
                return null;
            return _textureList[index];
        }

        public Texture2D GetRandomTexture()
        {
            int randomIndex = Random.Range(0, TextureCount);
            return _textureList[randomIndex];
        }
    }
}
