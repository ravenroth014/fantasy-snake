using System.Collections;
using FS_Runtimes.Controllers.Pooling;
using FS_Runtimes.Controllers.Utilities;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.Controllers.Character
{
    public class CharacterGameObject : MonoBehaviour
    {
        #region Fields & Properites

        [Header("Character Fields")]
        [SerializeField, Tooltip("Character Setting")] private CharacterGameObjectSetting _characterGameObjectSetting;
        [SerializeField, Tooltip("Character Mesh Renderer")] private SkinnedMeshRenderer _meshRenderer;
        [SerializeField, Tooltip("Character Animator")] private Animator _animator;

        private bool _isMoving;
        private bool _isInitOnCreate;
        private CharacterPooling _pooling;
        
        public Vector2 CurrentPosition { get; private set; }
        
        public string UniqueID { get; private set; }

        #endregion

        #region Methods

        public void InitOnCreate(CharacterPooling pooling, string uniqueID)
        {
            if (_isInitOnCreate) return;

            UniqueID = uniqueID;
            _pooling = pooling;
            _isInitOnCreate = true;
        }
        
        public void InitData(Vector2 position)
        {
            SetCharacterMaterial();
            SetCharacterPosition(position);
            SetCharacterAnimation(GameHelper.IdleState);
            
            LogManager.Instance.Log($"{this}'s initialization is complete.");
        }

        private void SetCharacterMaterial()
        {
            Texture2D texture2D = _characterGameObjectSetting.GetRandomTexture();
            Material material = _meshRenderer.material;
            
            if (material.HasProperty(GameHelper.MainTexture))
                material.SetTexture(GameHelper.MainTexture, texture2D);
            else
                LogManager.Instance.LogError("Material does not have an Albedo texture slot");
        }

        private void SetCharacterAnimation(string stateName)
        {
            _animator.Play(stateName);
        }

        private void SetCharacterDirection(EDirection direction)
        {
            Vector3 directionVector = GameHelper.GetWorldSpaceDirection(direction);
            if (directionVector == Vector3.zero) return;

            Quaternion rotation = Quaternion.LookRotation(directionVector);
            transform.rotation = rotation;
        }

        public void SetCharacterPosition(Vector2 position)
        {
            float height = transform.position.y;
            gameObject.transform.position = new Vector3(position.x, height, position.y);
        }

        public void MoveCharacterPosition(EDirection direction)
        {
            if (_isMoving) return;
            
            Vector3 directionVector = GameHelper.GetWorldSpaceDirection(direction);
            if (directionVector == Vector3.zero) return;

            _isMoving = true;
            Vector3 destination = gameObject.transform.position + directionVector;
            
            SetCharacterDirection(direction);
            SetCharacterAnimation(GameHelper.RunState);
            StartCoroutine(MoveCharacter(directionVector, destination));
        }

        public void MoveCharacterPosition(Vector2 targetPos)
        {
            if (_isMoving) return;
            
            _isMoving = true;
            Vector3 directionVector = new Vector3(targetPos.x, 0, targetPos.y);
            Vector3 destination = gameObject.transform.position + directionVector;
            EDirection direction = GameHelper.GetDirection(directionVector);
            
            SetCharacterDirection(direction);
            SetCharacterAnimation(GameHelper.RunState);
            StartCoroutine(MoveCharacter(directionVector, destination));
        }

        private IEnumerator MoveCharacter(Vector3 direction, Vector3 destination)
        {
            while (Vector3.Distance(transform.position, destination) > _characterGameObjectSetting.Threshold)
            {
                transform.position += direction * (_characterGameObjectSetting.Speed * Time.deltaTime);
                yield return null;
            }

            SetCharacterAnimation(GameHelper.IdleState);
            transform.position = destination;
            CurrentPosition = new Vector2(destination.x, destination.z);
            _isMoving = false;
        }

        public void Release()
        {
            if (_pooling is null)
                Destroy(gameObject);
            else
                _pooling.ReturnItemToPool(this);
        }

        #endregion
    }
}
