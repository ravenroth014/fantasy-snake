using System;
using System.Collections.Generic;
using System.Linq;
using FS_Runtimes.Models.Characters;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.Controllers.Character
{
    public class CharactersManager : MonoBehaviour
    {
        #region Fields & Properties

        private readonly List<CharacterData> _heroDataList = new();
        private readonly Dictionary<string, CharacterGameObject> _heroGameObjectDict = new();

        public bool IsMoving => _heroGameObjectDict.Values.ToList().Any(character => character.IsMoving);
        
        #endregion

        #region Methods

        public void Init()
        {
            
        }

        public void AddCharacter(CharacterGameObject newCharacter, ECharacterType characterType, EDirection direction = EDirection.None, Action<Vector2, string> onUpdateGrid = null)
        {
            string uniqueID = newCharacter.UniqueID;
            
            // TODO: Generate character data base on stage level.
            CharacterData newData = new CharacterData
            {
                UniqueID = uniqueID
                , CharacterType =  characterType
                , Level = 1
                , AttackPoint = 1
                , HealthPoint = 5
            };

            if (_heroGameObjectDict is { Count: > 0 })
            {
                string lastUniqueID = _heroDataList[^1].UniqueID;
                Vector2 lastPos = _heroGameObjectDict[lastUniqueID].CurrentPosition;
                MoveCharacter(direction, onUpdateGrid);
                newCharacter.SetCharacterPosition(lastPos);
                onUpdateGrid?.Invoke(lastPos, uniqueID);
            }
            
            _heroGameObjectDict[uniqueID] = newCharacter;
            _heroDataList.Add(newData);
        }

        public void MoveCharacter(EDirection direction, Action<Vector2,string> onUpdateGrid = null)
        {
            Vector2 cachePos = Vector2.zero;

            for (int index = 0; index < _heroGameObjectDict.Count; index++)
            {
                string uniqueID = _heroDataList[index].UniqueID;
                Vector2 targetPos;
                
                if (index == 0)
                {
                    Vector2 directionVector = GameHelper.Get2DDirection(direction);
                    cachePos = _heroGameObjectDict[uniqueID].CurrentPosition;
                    targetPos = cachePos + directionVector;
                    _heroGameObjectDict[uniqueID].MoveCharacterPosition(targetPos);
                }
                else
                {
                    targetPos = cachePos;
                    cachePos = _heroGameObjectDict[uniqueID].CurrentPosition;
                    _heroGameObjectDict[uniqueID].MoveCharacterPosition(targetPos);
                }
                
                onUpdateGrid?.Invoke(targetPos, uniqueID);
            }

            onUpdateGrid?.Invoke(cachePos, string.Empty);
        }

        public void SwitchCharacter(ECharacterSwitch switchDirection)
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

        private void RemoveCharacter(string uniqueID)
        {
            if (string.IsNullOrEmpty(uniqueID)) return;
            
            CharacterData characterData = _heroDataList.Find(data => data.UniqueID == uniqueID);
            _heroDataList.Remove(characterData);

            if (_heroGameObjectDict.ContainsKey(uniqueID))
            {
                CharacterGameObject removeCharacter = _heroGameObjectDict[uniqueID];
                _heroGameObjectDict.Remove(uniqueID);
                removeCharacter.Release();
            }
        }

        public void RemoveMainCharacter(EDirection direction, Action<Vector2, string> onUpdateGrid = null)
        {
            if (_heroDataList is null or {Count: 0})
                return;

            string uniqueID = _heroDataList[0].UniqueID;
            MoveCharacter(direction, onUpdateGrid);
            RemoveCharacter(uniqueID);
        }

        public Vector2 GetMainCharacterPosition()
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
            _heroDataList.Clear();

            if (_heroGameObjectDict is { Count: > 0 })
            {
                _heroGameObjectDict.Values.ToList().ForEach(hero => hero.Release());
                _heroGameObjectDict.Clear();
            }
        }

        #endregion
    }
}
