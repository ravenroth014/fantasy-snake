using System.Collections;
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

        private bool _isMoving;

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

        public void SetCharacterDirection(EDirection direction)
        {
            Vector3 directionVector = DirectionHelper.GetDirection(direction);
            if (directionVector == Vector3.zero) return;

            Quaternion rotation = Quaternion.LookRotation(directionVector);
            transform.rotation = rotation;
        }

        public void SetCharacterPosition(EDirection direction)
        {
            if (_isMoving) return;
            
            Vector3 directionVector = DirectionHelper.GetDirection(direction);
            if (directionVector == Vector3.zero) return;

            _isMoving = true;
            Vector3 destination = transform.position + directionVector;
            
            SetCharacterDirection(direction);
            SetCharacterAnimation(CharacterAnimationHelper.RunState);
            StartCoroutine(MoveCharacter(directionVector, destination));
        }

        private IEnumerator MoveCharacter(Vector3 direction, Vector3 destination)
        {
            while (Vector3.Distance(transform.position, destination) > _characterSetting.Threshold)
            {
                transform.position += direction * (_characterSetting.Speed * Time.deltaTime);
                yield return null;
            }

            SetCharacterAnimation(CharacterAnimationHelper.IdleState);
            transform.position = destination;
            _isMoving = false;
        }

        #endregion
    }
}
