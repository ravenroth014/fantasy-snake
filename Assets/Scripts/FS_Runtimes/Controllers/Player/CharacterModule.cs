using FS_Runtimes.Controllers.Utilities;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.Controllers.Player
{
    public class CharacterModule : MonoBehaviour
    {
        #region Fields & Properites

        [Header("Character Fields")]
        [SerializeField, Tooltip("Character Setting")] private CharacterSetting _characterSetting;
        [SerializeField, Tooltip("Character Mesh Renderer")] private SkinnedMeshRenderer _meshRenderer;
        [SerializeField, Tooltip("Character Animator")] private Animator _animator;

        #endregion

        #region Methods

        public void Init()
        {
            SetCharacterMaterial();
            SetCharacterAnimation(CharacterAnimationHelper.IdleState);
            
            LogManager.Instance.Log($"{this}'s initialization is complete.");
        }

        private void SetCharacterMaterial()
        {
            Texture2D texture2D = _characterSetting.GetRandomTexture();
            Material material = _meshRenderer.material;
            
            if (material.HasProperty(MaterialPropertyHelper.MainTexture))
                material.SetTexture(MaterialPropertyHelper.MainTexture, texture2D);
            else
                LogManager.Instance.LogError("Material does not have an Albedo texture slot");
        }

        public void SetCharacterAnimation(string stateName)
        {
            _animator.Play(stateName);
        }

        public void RotateCharacter()
        {
            
        }

        #endregion
    }
}
