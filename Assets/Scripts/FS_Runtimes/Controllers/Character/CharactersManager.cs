using System.Collections.Generic;
using System.Linq;
using System.Text;
using FS_Runtimes.Models.Characters;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.Controllers.Character
{
    public class CharactersManager : MonoBehaviour
    {
        #region Fields & Properties

        private readonly List<CharacterData> _heroDataList = new();
        private readonly List<CharacterData> _enemyDataList = new();

        private readonly Dictionary<string, CharacterGameObject> _heroGameObjectDict = new();
        private readonly Dictionary<string, CharacterGameObject> _enemyGameObjectDict = new();

        private int _currentHeroUniqueID = 0;
        private int _currentEnemyUniqueID = 0;

        private readonly string _enemyPrefix = "E";
        private readonly string _heroPrefix = "H";

        private StringBuilder _stringBuilder;

        #endregion

        #region Methods

        public void Init()
        {
            _currentHeroUniqueID = 0;
            _currentEnemyUniqueID = 0;
            _stringBuilder = new StringBuilder();
        }

        public string AddCharacter(CharacterGameObject characterGameObject, ECharacterType characterType, Vector2 position)
        {
            _stringBuilder ??= new StringBuilder();
            _stringBuilder.Clear();

            string uniqueID = string.Empty;
            
            // TODO: Generate character data base on stage level.
            CharacterData newData = new CharacterData
            {
                UniqueID = uniqueID
                , CharacterType =  characterType
                , Level = 1
                , AttackPoint = 1
                , HealthPoint = 5
            };

            if (characterType == ECharacterType.Enemy)
            {
                _stringBuilder.Append(_enemyPrefix);
                _stringBuilder.Append(_currentEnemyUniqueID.ToString("0000"));
                _currentEnemyUniqueID++;
                
                uniqueID = _stringBuilder.ToString();
                newData.UniqueID = uniqueID;

                _enemyGameObjectDict[uniqueID] = characterGameObject;
                _enemyDataList.Add(newData);
            }
            else if (characterType == ECharacterType.Hero)
            {
                _stringBuilder.Append(_heroPrefix);
                _stringBuilder.Append(_currentHeroUniqueID.ToString("0000"));
                _currentHeroUniqueID++;
                
                uniqueID = _stringBuilder.ToString();
                newData.UniqueID = uniqueID;

                _heroGameObjectDict[uniqueID] = characterGameObject;
                _heroDataList.Add(newData);
            }
            
            characterGameObject.InitData(uniqueID, position);
            

            return uniqueID;
        }

        public void MoveHeroCharacter(EDirection direction)
        {
            Vector2 cachePos = Vector2.zero;

            for (int index = 0; index < _heroGameObjectDict.Count; index++)
            {
                string uniqueID = _heroDataList[index].UniqueID;
                Vector2 targetPos = cachePos;
                cachePos = _heroGameObjectDict[uniqueID].CurrentPosition;
                
                if (index == 0)
                {
                    _heroGameObjectDict[uniqueID].MoveCharacterPosition(direction);
                }
                else
                {
                    _heroGameObjectDict[uniqueID].MoveCharacterPosition(targetPos);
                }
            }
        }

        public void SwitchHeroCharacter(ECharacterSwitch switchDirection)
        {
            if (_heroDataList is null or {Count: 0})
                return;
            
            if (switchDirection == ECharacterSwitch.Left)
            {
                CharacterData data = _heroDataList[0];
                _heroDataList.RemoveAt(0);
                _heroDataList.Add(data);
            }
            else if (switchDirection == ECharacterSwitch.Right)
            {
                int index = _heroDataList.Count - 1;
                CharacterData data = _heroDataList[index];
                _heroDataList.RemoveAt(index);
                _heroDataList.Insert(0, data);
            }
        }

        public void RemoveCharacter(string uniqueID, ECharacterType characterType)
        {
            if (string.IsNullOrEmpty(uniqueID)) return;
            
            if (characterType == ECharacterType.Hero)
            {
                CharacterData characterData = _heroDataList.Find(data => data.UniqueID == uniqueID);
                _heroDataList.Remove(characterData);

                if (_heroGameObjectDict.ContainsKey(uniqueID))
                    _heroGameObjectDict.Remove(uniqueID);
            }
            else if (characterType == ECharacterType.Enemy)
            {
                CharacterData characterData = _enemyDataList.Find(data => data.UniqueID == uniqueID);
                _enemyDataList.Remove(characterData);

                if (_enemyGameObjectDict.ContainsKey(uniqueID))
                    _enemyGameObjectDict.Remove(uniqueID);
            }
        }

        public Vector2 GetCurrentHeroPosition()
        {
            if (_heroDataList is null or { Count: 0 }) 
                return Vector2.zero;
            string uniqueID = _heroDataList[0].UniqueID;

            if (_heroGameObjectDict.TryGetValue(uniqueID, out CharacterGameObject character) == false) 
                return Vector2.zero;
            return character.CurrentPosition;
        }
        
        public void ResetManager()
        {
            _currentHeroUniqueID = 0;
            _currentEnemyUniqueID = 0;
            
            _heroDataList.Clear();
            _enemyDataList.Clear();

            if (_heroGameObjectDict is { Count: > 0 })
            {
                _heroGameObjectDict.Values.ToList().ForEach(hero => hero.Release());
                _heroGameObjectDict.Clear();
            }

            if (_enemyGameObjectDict is { Count: > 0 })
            {
                _enemyGameObjectDict.Values.ToList().ForEach(enemy => enemy.Release());
                _enemyGameObjectDict.Clear();
            }
        }

        #endregion
    }
}
