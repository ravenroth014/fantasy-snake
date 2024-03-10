using System.Collections.Generic;
using System.Linq;
using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Pooling;
using FS_Runtimes.Models.Levels;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.Controllers.Level
{
    public class LevelManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Pooling")]
        [SerializeField, Tooltip("Decorate Pooling")] private DecoratePooling _decoratePooling;
        [SerializeField, Tooltip("Obstacle Pooling")] private DecoratePooling _obstaclePooling;
        [SerializeField, Tooltip("Hero Pooling")] private CharacterPooling _heroPooling;
        [SerializeField, Tooltip("Enemy Pooling")] private CharacterPooling _enemyPooling;
        
        private readonly int _horizontalMaxSize = 16;
        private readonly int _verticalMaxSize = 16;
        private readonly Dictionary<Vector2, GridData> _gridDict = new();
        
        private CharacterGameObject _enlistCharacter;
        private CharacterGameObject _enemyCharacter;

        #endregion

        #region Methods

        public void Init()
        {
            GenerateDict();
            
            _decoratePooling.Init();
            _obstaclePooling.Init();
            _heroPooling.Init();
            _enemyPooling.Init();
        }
        
        private void GenerateDict()
        {
            for (int hIndex = 0; hIndex <= _horizontalMaxSize + 1; hIndex++)
            {
                for (int vIndex = 0; vIndex <= _verticalMaxSize + 1; vIndex++)
                {
                    EGridState gridState = EGridState.Empty;

                    if (hIndex == 0 || hIndex == _horizontalMaxSize + 1 || vIndex == 0 || vIndex == _verticalMaxSize + 1)
                        gridState = EGridState.Walled;
                    
                    Vector2 newVector = new Vector2(hIndex, vIndex);
                    _gridDict.Add(newVector, new GridData(newVector, gridState));
                }
            }
        }
        
        public void ResetManager()
        {
            if (_gridDict is null or { Count: 0 })
                GenerateDict();
            else
                _gridDict.Values.ToList().ForEach(grid => grid.Reset());
            
        }

        private Vector2 GetFreePosition()
        {
            List<GridData> freeGridList = _gridDict.Values.Where(grid => grid.GridState == EGridState.Empty).ToList();

            if (freeGridList is null or { Count: 0 })
                return Vector2.zero;
            
            int randIndex = Random.Range(0, freeGridList.Count);
            return freeGridList[randIndex].GridIndex;
        }

        public void UpdateGridData(Vector2 position, string uniqueID, EGridState gridState, ECharacterType characterType)
        {
            if (_gridDict.TryGetValue(position, out GridData gridData))
            {
                gridData.UpdateData(uniqueID, gridState, characterType);
            }
        }

        public void GenerateEnlist()
        {
            CharacterGameObject character = _heroPooling.GetFromPool();
            Vector2 position = GetFreePosition();

            UpdateGridData(position, character.UniqueID, EGridState.Occupied, ECharacterType.Enlist);

            _enlistCharacter = character;
            _enlistCharacter.InitData(position);
        }

        public void GenerateEnemy()
        {
            CharacterGameObject character = _enemyPooling.GetFromPool();
            Vector2 position = GetFreePosition();

            UpdateGridData(position, character.UniqueID, EGridState.Occupied, ECharacterType.Enemy);

            _enemyCharacter = character;
            _enemyCharacter.InitData(position);
        }

        public void GenerateObstacle()
        {
            
        }

        public CharacterGameObject GenerateHero()
        {
            CharacterGameObject character = _heroPooling.GetFromPool();
            Vector2 position = GetFreePosition();
            
            UpdateGridData(position, character.UniqueID, EGridState.Occupied, ECharacterType.Hero);
            
            character.InitData(position);
            return character;
        }

        public EGridState GetGridState(Vector2 position)
        {
            return _gridDict[position].GridState;
        }

        public ECharacterType GetGridOccupiedType(Vector2 position)
        {
            return _gridDict[position].CharacterType;
        }

        public CharacterGameObject GetGridOccupiedCharacter(Vector2 position)
        {
            CharacterGameObject character;
            
            if (GetGridOccupiedType(position) == ECharacterType.Enlist)
            {
                character = _enlistCharacter;
                _enlistCharacter = null;
                return character;
            }
            if (GetGridOccupiedType(position) == ECharacterType.Enemy)
            {
                character = _enemyCharacter;
                _enemyCharacter = null;
                return character;
            }
            return null;
        }
        
        #endregion
    }
}
