using System.Collections.Generic;
using System.Linq;
using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Decorate;
using FS_Runtimes.Controllers.Pooling;
using FS_Runtimes.Controllers.Utilities;
using FS_Runtimes.Models.Characters;
using FS_Runtimes.Models.Levels;
using FS_Runtimes.Utilities;
using FS_Runtimes.Utilities.Setting;
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

        [Header("Pool Parent")] 
        [SerializeField, Tooltip("Enlist parent")] private Transform _enlistParent;
        [SerializeField, Tooltip("Enemy parent")] private Transform _enemyParent;
        [SerializeField, Tooltip("Obstacle parent")] private Transform _obstacleParent;
        [SerializeField, Tooltip("Decorate parent")] private Transform _decorateParent;
        
        [Header("Grid Generate Rule Set")] 
        [SerializeField, Tooltip("Obstacle Setting")] private ObstacleSetting _obstacleSetting;
        [SerializeField, Tooltip("Decorate Setting")] private ObstacleSetting _decorateSetting;

        [Header("Character Stat Data")] 
        [SerializeField, Tooltip("Hero Base Stat")] private List<CharacterBaseStat> _heroStatList;
        [SerializeField, Tooltip("Enemy Base Stat")] private List<CharacterBaseStat> _enemyStatList;

        [Header("Level Objective Setting")] 
        [SerializeField, Tooltip("Level Objective Setting")] private LevelObjectiveSetting _levelObjectiveSetting;
        
        private readonly int _horizontalMaxSize = 16;
        private readonly int _verticalMaxSize = 16;
        private readonly Dictionary<Vector2, GridData> _gridDict = new();
        private readonly List<DecorateGameObject> _obstacleList = new();
        private readonly List<DecorateGameObject> _decorateList = new();
        
        private CharacterPairData _enlistCharacter;
        private CharacterPairData _enemyCharacter;

        #endregion

        #region Methods

        #region Init Methods
        
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
        
        #endregion

        #region Utility Methods
        
        public void ResetManager()
        {
            if (_gridDict is null or { Count: 0 })
                GenerateDict();
            else
                _gridDict.Values.ToList().ForEach(grid => grid.Reset());
            
            _obstacleList.ForEach(obstacle => obstacle.Release());
            _obstacleList.Clear();
            
            _decorateList.ForEach(decorate => decorate.Release());
            _decorateList.Clear();
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
        
        public EGridState GetGridState(Vector2 position)
        {
            return _gridDict[position].GridState;
        }

        public ECharacterType GetGridOccupiedType(Vector2 position)
        {
            return _gridDict[position].CharacterType;
        }

        public int GetCurrentLevelObjective(int level)
        {
            return (int)(_levelObjectiveSetting.BaseTarget * Mathf.Pow(level, _levelObjectiveSetting.LevelExponential));
        }
        
        #endregion

        #region Generate Methods

        public void GenerateLevel()
        {
            LogManager.Instance.Log("Generating obstacle decoration...");
            GenerateObstacle();
            
            LogManager.Instance.Log("Generating level decoration...");
            GenerateDecoration();
        }
        
        public CharacterPairData GenerateHero(int level = 1)
        {
            CharacterGameObject character = _heroPooling.GetFromPool();
            CharacterData data = GenerateCharacterData(ECharacterType.Hero, character.UniqueID, level);
            Vector2 position = GetFreePosition();
            
            UpdateGridData(position, character.UniqueID, EGridState.Occupied, ECharacterType.Hero);
            
            character.InitData(position);

            CharacterPairData pairData = new CharacterPairData(data, character);
            
            return pairData;
        }
        
        public void GenerateEnlist(int level = 1)
        {
            CharacterGameObject character = _heroPooling.GetFromPool();
            CharacterData data = GenerateCharacterData(ECharacterType.Enlist, character.UniqueID, level);
            Vector2 position = GetFreePosition();

            UpdateGridData(position, character.UniqueID, EGridState.Occupied, ECharacterType.Enlist);
            character.InitData(position);
            character.SetParent(_enlistParent);

            CharacterPairData pairData = new CharacterPairData(data, character);
            _enlistCharacter = pairData;
        }

        public void GenerateEnemy(int level = 1)
        {
            CharacterGameObject character = _enemyPooling.GetFromPool();
            CharacterData data = GenerateCharacterData(ECharacterType.Enemy, character.UniqueID, level);
            Vector2 position = GetFreePosition();

            UpdateGridData(position, character.UniqueID, EGridState.Occupied, ECharacterType.Enemy);
            character.InitData(position);
            character.SetHighlightState(true);
            character.SetParent(_enemyParent);

            CharacterPairData pairData = new CharacterPairData(data, character);
            _enemyCharacter = pairData;
        }

        private void GenerateDecoration()
        {
            List<GridData> availableGrid = _gridDict.Values.Where(gridData => gridData.GridState == EGridState.Empty).ToList();

            for(int i = 0; i < _decorateSetting.TotalObstacle; i++)
            {
                if (availableGrid is null or {Count: 0})
                    return;

                int randGridIndex = Random.Range(0, availableGrid.Count);
                GridData selectedGrid = availableGrid[randGridIndex];
                availableGrid.RemoveAt(randGridIndex);
                
                DecorateGameObject decorate = _decoratePooling.GetFromPool();
                decorate.SetDecoratePosition(selectedGrid.GridIndex);
                decorate.SetParent(_decorateParent);
                _decorateList.Add(decorate);
            }
        }

        private void GenerateObstacle()
        {
            List<GridData> availableGrid = _gridDict.Values.Where(gridData => gridData.GridState == EGridState.Empty).ToList();
            int totalObstacle = _obstacleSetting.TotalObstacle;
            
            do
            {
                if (availableGrid is null or {Count: 0})
                    return;

                int randGridIndex = Random.Range(0, availableGrid.Count);
                GridData selectedGrid = availableGrid[randGridIndex];
                availableGrid.RemoveAt(randGridIndex);

                int sizeX = Random.Range(1, _obstacleSetting.MaxWidthSize + 1);
                int sizeY = Random.Range(1, _obstacleSetting.MaxHeightSize + 1);
                int obstacleSize = sizeX * sizeY;
                
                if (obstacleSize > totalObstacle)
                {
                    sizeX = 1;
                    sizeY = 1;
                    obstacleSize = 1;
                }

                if (CanPlaceObstacle(selectedGrid, sizeX, sizeY) == false)
                    continue;

                GenerateObstacle(availableGrid, selectedGrid, sizeX, sizeY);
                totalObstacle -= obstacleSize;
                
            } while (totalObstacle > 0);
        }

        private void GenerateObstacle(List<GridData> availableGrid, GridData gridData, int sizeX, int sizeY)
        {
            if (availableGrid is null or {Count: 0})
                return;
            
            if (gridData == null)
                return;

            Vector2 basePos = gridData.GridIndex;
            
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    DecorateGameObject obstacle = _obstaclePooling.GetFromPool();
                    Vector2 position = basePos + new Vector2(x, y);

                    obstacle.SetDecoratePosition(position);
                    obstacle.SetParent(_obstacleParent);
                    UpdateGridData(position, string.Empty, EGridState.Obstacle, ECharacterType.None);
                    
                    _obstacleList.Add(obstacle);
                }
            }
        }

        private bool CanPlaceObstacle(GridData selectedGrid, int sizeX, int sizeY)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    Vector2 key = new Vector2(selectedGrid.GridIndex.x + x, selectedGrid.GridIndex.y + y);
                    if (_gridDict[key].GridState != EGridState.Empty)
                        return false;
                }
            }

            return true;
        }
        
        public CharacterPairData GetEnlistCharacter(int level)
        {
            CharacterPairData character = _enlistCharacter;
            character.CharacterData.UpdateCharacterStat(level);
            _enlistCharacter = null;
            return character;
        }

        public CharacterPairData GetEnemyCharacter()
        {
            return _enemyCharacter;
        }

        private CharacterData GenerateCharacterData(ECharacterType characterType, string uniqueID, int level)
        {
            switch (characterType)
            {
                case ECharacterType.Enlist or ECharacterType.Hero when _heroStatList is null or { Count: 0 }:
                case ECharacterType.Enemy when _enemyStatList is null or { Count: 0}:
                {
                    return null;
                }
                case ECharacterType.Enlist or ECharacterType.Hero:
                {
                    int randIndex = Random.Range(0, _heroStatList.Count);
                    CharacterData newData = new CharacterData(_heroStatList[randIndex], uniqueID, level);
                    return newData;
                }
                case ECharacterType.Enemy:
                {
                    int randIndex = Random.Range(0, _enemyStatList.Count);
                    CharacterData newData = new CharacterData(_enemyStatList[randIndex], uniqueID, level);
                    return newData;
                }
                default:
                    return null;
            }
        }

        public void RemoveEnemy(Vector2 targetPos)
        {
            UpdateGridData(targetPos, string.Empty, EGridState.Empty, ECharacterType.None);
            _enemyCharacter.CharacterGameObject.Release();
            _enemyCharacter = null;
        }
        
        #endregion
        
        #endregion
    }
}
