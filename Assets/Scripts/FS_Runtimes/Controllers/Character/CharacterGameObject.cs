using System.Collections;
using FS_Runtimes.Controllers.Pooling;
using FS_Runtimes.Controllers.Utilities;
using FS_Runtimes.Utilities;
using FS_Runtimes.Utilities.Setting;
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
        [SerializeField, Tooltip("Character Highlight")] private GameObject _highlight;

        private bool _isMoving;
        private bool _isInitOnCreate;
        private CharacterPooling _pooling;
        
        public Vector2 CurrentPosition { get; private set; }
        public Vector3 CurrentDirection { get; private set; }
        public string UniqueID { get; private set; }
        public bool IsMoving => _isMoving;

        #endregion

        #region Methods

        #region Init Methods
        
        /// <summary>
        /// Call this method when this character game object is firstly created from pool.
        /// </summary>
        /// <param name="pooling"></param>
        /// <param name="uniqueID"></param>
        public void InitOnCreate(CharacterPooling pooling, string uniqueID)
        {
            if (_isInitOnCreate) return;

            UniqueID = uniqueID;
            _pooling = pooling;
            _isInitOnCreate = true;
            _highlight.SetActive(false);
        }
        
        /// <summary>
        /// Call this method to initialize character state, such as skin, position and animation.
        /// </summary>
        /// <param name="position"></param>
        public void InitData(Vector2 position)
        {
            SetCharacterMaterial();
            SetCharacterPosition(position);
            SetCharacterAnimation(GameHelper.IdleState);
            
            LogManager.Instance.Log($"{UniqueID}'s initialization is complete.");
        }
        
        #endregion

        #region Set Methods
        
        /// <summary>
        /// Call this method to set character skin.
        /// </summary>
        private void SetCharacterMaterial()
        {
            Texture2D texture2D = _characterGameObjectSetting.GetRandomTexture();
            Material material = _meshRenderer.material;
            
            if (material.HasProperty(GameHelper.MainTexture))
                material.SetTexture(GameHelper.MainTexture, texture2D);
            else
                LogManager.Instance.LogError("Material does not have an Albedo texture slot");
        }

        /// <summary>
        /// Call this method to set character animation
        /// </summary>
        /// <param name="stateName"></param>
        private void SetCharacterAnimation(string stateName)
        {
            _animator.Play(stateName);
        }

        /// <summary>
        /// Call this method to set character direction with Vector2
        /// </summary>
        /// <param name="targetPos"></param>
        public void SetCharacterDirection(Vector2 targetPos)
        {
            Vector3 direction = new Vector3(targetPos.x - CurrentPosition.x, 0, targetPos.y - CurrentPosition.y);
            SetCharacterDirection(direction);
        }
        
        /// <summary>
        /// Call this method to set character direction with Vector3
        /// </summary>
        /// <param name="direction"></param>
        public void SetCharacterDirection(Vector3 direction)
        {
            if (direction == Vector3.zero) return;

            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
            CurrentDirection = direction;
        }

        /// <summary>
        /// Call this method to set character position.
        /// </summary>
        /// <param name="position"></param>
        public void SetCharacterPosition(Vector2 position)
        {
            float height = transform.position.y;
            gameObject.transform.position = new Vector3(position.x, height, position.y);
            CurrentPosition = position;
        }
        
        /// <summary>
        /// Call this method to set highlight item object state.
        /// </summary>
        /// <param name="state"></param>
        public void SetHighlightState(bool state)
        {
            if (_highlight is null)
                return;
            
            _highlight.SetActive(state);
        }

        /// <summary>
        /// Call this method to set parent of this object.
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(Transform parent)
        {
            gameObject.transform.parent = parent;
        }
        
        #endregion

        #region Control Methods
        
        /// <summary>
        /// Call this method to move character to target position
        /// </summary>
        /// <param name="targetPos"></param>
        public void MoveCharacterPosition(Vector2 targetPos)
        {
            if (_isMoving) return;
            
            _isMoving = true;
            Vector3 direction = new Vector3(targetPos.x - CurrentPosition.x, 0, targetPos.y - CurrentPosition.y);
            Vector3 destination = gameObject.transform.position + direction;
            
            SetCharacterDirection(direction);
            SetCharacterAnimation(GameHelper.RunState);
            StartCoroutine(MoveCharacter(direction, destination));
        }

        /// <summary>
        /// Call this method to move character until it reach to destination.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
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
            LogManager.Instance.Log($"Unique ID: {UniqueID}, Current position : {CurrentPosition.ToString()}");
            _isMoving = false;
        }
        
        #endregion

        #region Utility Methods
        
        /// <summary>
        /// Call this method to return this item to its pool.
        /// </summary>
        public void Release()
        {
            if (_pooling is null)
                Destroy(gameObject);
            else
            {
                _isMoving = false;
                _pooling.ReturnItemToPool(this);
                SetHighlightState(false);
            }
        }

        #endregion

        #endregion
    }
}
