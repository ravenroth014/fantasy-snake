using System.Collections.Generic;
using System.Linq;
using FS_Runtimes.Controllers.Character;
using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Controllers.Decorate;
using FS_Runtimes.Controllers.Pooling;
using FS_Runtimes.Controllers.Utilities;
using FS_Runtimes.Models.Characters;
using FS_Runtimes.Models.Levels;
using FS_Runtimes.Models.Settings;
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

        private readonly int _horizontalMaxSize = 16;
        private readonly int _verticalMaxSize = 16;
        private readonly List<DecorateGameObject> _obstacleList = new();
        private readonly List<DecorateGameObject> _decorateList = new();
        private readonly Dictionary<Vector2, GridData> _gridDict = new();
        private readonly Dictionary<Vector2, CharacterPairData> _enlistCharacterDict = new();
        private readonly Dictionary<Vector2, CharacterPairData> _enemyCharacterDict = new();

        private PersistenceGameSetting _levelSetting;
        
        public int TotalKillEnemies { get; private set; }

        #endregion

        #region Methods

        #region Init Methods
        
        /// <summary>
        /// Call this method to initialize data and pooling object.
        /// </summary>
        public void Init()
        {
            GenerateDict();
            
            _decoratePooling.Init();
            _obstaclePooling.Init();
            _heroPooling.Init();
            _enemyPooling.Init();
        }
        
        /// <summary>
        /// Call this method to generate grid data dictionary.
        /// </summary>
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

        /// <summary>
        /// Call this method to reset back to its initial state.
        /// </summary>
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
            
            if (_enlistCharacterDict is {Count: > 0})
            {
                _enlistCharacterDict.Values.ToList().ForEach(enlist => enlist.CharacterGameObject.Release());
                _enlistCharacterDict.Clear();
            }

            if (_enemyCharacterDict is { Count: > 0 })
            {
                _enemyCharacterDict.Values.ToList().ForEach(enemy => enemy.CharacterGameObject.Release());
                _enemyCharacterDict.Clear();
            }

            TotalKillEnemies = 0;
        }
        
        /// <summary>
        /// Call this method to update grid data.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="uniqueID"></param>
        /// <param name="gridState"></param>
        /// <param name="characterType"></param>
        public void UpdateGridData(Vector2 position, string uniqueID, EGridState gridState, ECharacterType characterType)
        {
            if (_gridDict.TryGetValue(position, out GridData gridData))
            {
                gridData.UpdateData(uniqueID, gridState, characterType);
            }
        }

        #endregion

        #region Get Methods
        
        /// <summary>
        /// Call this method to get random free grid.
        /// </summary>
        /// <returns></returns>
        private Vector2 GetFreePosition()
        {
            List<GridData> freeGridList = _gridDict.Values.Where(grid => grid.GridState == EGridState.Empty).ToList();

            if (freeGridList is null or { Count: 0 })
                return Vector2.zero;
            
            int randIndex = Random.Range(0, freeGridList.Count);
            return freeGridList[randIndex].GridIndex;
        }
        
        /// <summary>
        /// Call this method to get grid state data.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public EGridState GetGridState(Vector2 position)
        {
            return _gridDict[position].GridState;
        }

        /// <summary>
        /// Call this method to get grid occupied state.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public ECharacterType GetGridOccupiedType(Vector2 position)
        {
            return _gridDict[position].CharacterType;
        }
        
        /// <summary>
        /// Call this method to fetch enlist by grid position.
        /// </summary>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        public CharacterPairData GetEnlistCharacter(Vector2 targetPos)
        {
            if (_enlistCharacterDict is { Count: > 0 } && _enlistCharacterDict.Remove(targetPos, out CharacterPairData enlist))
                return enlist;
            return null;
        }

        /// <summary>
        /// Call this method to get enemy data by grid position.
        /// </summary>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        public CharacterPairData GetEnemyCharacter(Vector2 targetPos)
        {
            return _enemyCharacterDict is {Count: > 0} && _enemyCharacterDict.TryGetValue(targetPos, out CharacterPairData enemy) ? enemy : null;
        }
        
        #endregion

        #region Generate Methods

        /// <summary>
        /// Call this method to generate level state.
        /// </summary>
        public void GenerateLevel()
        {
            _levelSetting = SettingManager.Instance.GetCurrentGameplaySetting();
            TotalKillEnemies = 0;
            
            LogManager.Instance.Log("Generating obstacle decoration...");
            GenerateObstacle();
            
            LogManager.Instance.Log("Generating level decoration...");
            GenerateDecoration();
        }
        
        /// <summary>
        /// Call this method to generate main hero at the initial state of game state.
        /// </summary>
        /// <returns></returns>
        public CharacterPairData GenerateHero()
        {
            CharacterGameObject character = _heroPooling.GetFromPool();
            CharacterData data = GenerateCharacterData(character.UniqueID);
            Vector2 position = GetFreePosition();
            
            UpdateGridData(position, character.UniqueID, EGridState.Occupied, ECharacterType.Hero);
            
            character.InitData(position);

            CharacterPairData pairData = new CharacterPairData(data, character);
            
            return pairData;
        }
        
        /// <summary>
        /// Call this method to generate enlist character into state.
        /// </summary>
        private void GenerateEnlist()
        {
            CharacterGameObject character = _heroPooling.GetFromPool();
            CharacterData data = GenerateCharacterData(character.UniqueID);
            Vector2 position = GetFreePosition();

            UpdateGridData(position, character.UniqueID, EGridState.Occupied, ECharacterType.Enlist);
            character.InitData(position);
            character.SetParent(_enlistParent);

            CharacterPairData pairData = new CharacterPairData(data, character);
            _enlistCharacterDict[position] = pairData;
        }

        /// <summary>
        /// Call this method to generate enemy character into state.
        /// </summary>
        private void GenerateEnemy()
        {
            CharacterGameObject character = _enemyPooling.GetFromPool();
            CharacterData data = GenerateCharacterData(character.UniqueID);
            Vector2 position = GetFreePosition();

            UpdateGridData(position, character.UniqueID, EGridState.Occupied, ECharacterType.Enemy);
            character.InitData(position);
            character.SetHighlightState(true);
            character.SetParent(_enemyParent);

            CharacterPairData pairData = new CharacterPairData(data, character);
            _enemyCharacterDict[position] = pairData;
        }

        /// <summary>
        /// Call this method to generate characters at the initial state.
        /// </summary>
        public void GenerateCharactersOnStart()
        {
            int totalSpawnable = _levelSetting.StartEntity;

            for (int i = 0; i < totalSpawnable; i++)
            {
                int randIndex = Random.Range(0, 2);
                if (randIndex == 0)
                    GenerateEnlist();
                else
                    GenerateEnemy();
            }
        }
        
        /// <summary>
        /// Call this method to generate characters at the progress state.
        /// </summary>
        public void GenerateCharacters()
        {
            int currentActiveEntity = _enlistCharacterDict.Count + _enemyCharacterDict.Count;
            
            if (currentActiveEntity >= _levelSetting.MaxActiveEntity)
                return;

            int totalSpawnable = _levelSetting.MaxActiveEntity - currentActiveEntity;
            totalSpawnable = totalSpawnable > _levelSetting.MaxSpawnable ? _levelSetting.MaxSpawnable : totalSpawnable;
            totalSpawnable = Random.Range(1, totalSpawnable + 1);

            if (_enemyCharacterDict.Count == 0 && totalSpawnable > 0)
            {
                GenerateEnemy();
                totalSpawnable--;
            }

            if (_enlistCharacterDict.Count == 0 && totalSpawnable > 0)
            {
                GenerateEnlist();
                totalSpawnable--;
            }

            for (int i = 0; i < totalSpawnable; i++)
            {
                int randIndex = Random.Range(0, 2);
                if (randIndex == 0)
                    GenerateEnlist();
                else
                    GenerateEnemy();
            }
        }

        /// <summary>
        /// Call this method to generate level state decoration.
        /// </summary>
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

        /// <summary>
        /// Call this method to generate level state obstacle.
        /// </summary>
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

        /// <summary>
        /// Call this method to generate level state obstacle.
        /// </summary>
        /// <param name="availableGrid"></param>
        /// <param name="gridData"></param>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
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

        /// <summary>
        /// Call this state to check if the surround area of current position can spawn obstacle or not.
        /// </summary>
        /// <param name="selectedGrid"></param>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Call this method to generate character data randomly.
        /// </summary>
        /// <param name="uniqueID"></param>
        /// <returns></returns>
        private CharacterData GenerateCharacterData(string uniqueID)
        {
            int attackStat = Random.Range(_levelSetting.MinAttack, _levelSetting.MaxAttack + 1);
            int healthStat = Random.Range(_levelSetting.MinHealth, _levelSetting.MaxHealth + 1);
            int growthByMove = _levelSetting.GrowthWithMove;
            float growthRate = _levelSetting.StatGrowthRate;

            return new CharacterData(attackStat, healthStat, growthRate, growthByMove, uniqueID);
        }
        
        #endregion
        
        #region Management Methods

        /// <summary>
        /// Call this method to remove enemy from level state.
        /// </summary>
        /// <param name="targetPos"></param>
        public void RemoveEnemy(Vector2 targetPos)
        {
            UpdateGridData(targetPos, string.Empty, EGridState.Empty, ECharacterType.None);
            
            if (_enemyCharacterDict is null or {Count: 0}) return;
            if (_enemyCharacterDict.TryGetValue(targetPos, out CharacterPairData enemyCharacter) == false) return;
            
            enemyCharacter.CharacterGameObject.Release();
            _enemyCharacterDict.Remove(targetPos);
            TotalKillEnemies++;
        }
        
        /// <summary>
        /// Call this method to execute action for enlist and enemy stat growing mechanic.
        /// </summary>
        public void OnTriggerActionEndPhase()
        {
            _enlistCharacterDict.Values.ToList().ForEach(enlist => enlist.CharacterData.OnTakeAction());
            _enemyCharacterDict.Values.ToList().ForEach(enemy => enemy.CharacterData.OnTakeAction());
        }
        
        #endregion
        
        #endregion
    }
}
