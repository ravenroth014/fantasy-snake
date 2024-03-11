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

        [Header("Player Line Reference")] 
        [SerializeField, Tooltip("Player Line Parent")] private Transform _playerLine;
        
        private readonly List<CharacterData> _heroDataList = new();
        private readonly Dictionary<string, CharacterGameObject> _heroGameObjectDict = new();

        public CharacterPairData CurrentMainHero { get; private set; }

        public bool IsMoving => _heroGameObjectDict.Values.ToList().Any(character => character.IsMoving);
        
        #endregion

        #region Methods

        public void Init()
        {
            
        }

        public void AddCharacter(CharacterPairData newCharacter, Vector2 targetPos, Action<Vector2, string> onUpdateGrid = null)
        {
            string uniqueID = newCharacter.CharacterData.UniqueID;

            if (_heroGameObjectDict is { Count: > 0 })
            {
                string lastUniqueID = _heroDataList[^1].UniqueID;
                Vector2 lastPos = _heroGameObjectDict[lastUniqueID].CurrentPosition;
                MoveCharacter(targetPos, onUpdateGrid);
                newCharacter.CharacterGameObject.SetCharacterPosition(lastPos);
                onUpdateGrid?.Invoke(lastPos, uniqueID);
            }
            else
            {
                newCharacter.CharacterGameObject.SetHighlightState(true);
                CurrentMainHero = newCharacter;
            }

            _heroGameObjectDict[uniqueID] = newCharacter.CharacterGameObject;
            _heroGameObjectDict[uniqueID].SetParent(_playerLine);
            _heroDataList.Add(newCharacter.CharacterData);
        }
        
        public void MoveCharacter(Vector2 targetPosition, Action<Vector2, string> onUpdateGrid = null)
        {
            Vector2 cachePos = Vector2.zero;

            for (int index = 0; index < _heroDataList.Count; index++)
            {
                string uniqueID = _heroDataList[index].UniqueID;

                if (index == 0)
                {
                    cachePos = _heroGameObjectDict[uniqueID].CurrentPosition;
                    _heroGameObjectDict[uniqueID].MoveCharacterPosition(targetPosition);
                }
                else
                {
                    Vector2 targetPos = cachePos;
                    cachePos = _heroGameObjectDict[uniqueID].CurrentPosition;
                    _heroGameObjectDict[uniqueID].MoveCharacterPosition(targetPos);
                }
                
                onUpdateGrid?.Invoke(cachePos, uniqueID);
            }
            
            onUpdateGrid?.Invoke(cachePos, string.Empty);
        }

        public void SwitchCharacter(ECharacterSwitch switchDirection, Action<Vector2, string> onUpdateGrid = null)
        {
            if (_heroDataList is null or {Count: <= 1})
                return;

            List<Vector2> cachePosList = new();
            List<Vector3> cacheDirList = new();
            
            _heroDataList.ForEach(characterData =>
            {
                cachePosList.Add(_heroGameObjectDict[characterData.UniqueID].CurrentPosition);
                cacheDirList.Add(_heroGameObjectDict[characterData.UniqueID].CurrentDirection);
            });
            
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

            string currentUniqueID = _heroDataList[0].UniqueID;
            CurrentMainHero.CharacterData = _heroDataList[0];
            CurrentMainHero.CharacterGameObject = _heroGameObjectDict[currentUniqueID];
            
            UpdateCharactersTransform(cachePosList, cacheDirList, onUpdateGrid);
            SetHighlightState();
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

        public void RemoveMainCharacter(Vector2 targetPosition, Action<Vector2, string> onUpdateGrid = null)
        {
            if (_heroDataList is null or {Count: 0})
                return;

            string uniqueID = _heroDataList[0].UniqueID;
            MoveCharacter(targetPosition, onUpdateGrid);
            RemoveCharacter(uniqueID);
            SetHighlightState();
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

            CurrentMainHero = null;
        }

        private void SetHighlightState()
        {
            if (_heroDataList is null or {Count: 0})
                return;
            
            string uniqueID = _heroDataList[0].UniqueID;
            
            foreach (KeyValuePair<string,CharacterGameObject> characterGameObject in _heroGameObjectDict)
            {
                characterGameObject.Value.SetHighlightState(characterGameObject.Key == uniqueID);
            }
        }

        private void UpdateCharactersTransform(List<Vector2> cachePosList, List<Vector3> cacheDirList, Action<Vector2, string> onUpdateGrid = null)
        {
            for (int index = 0; index < cachePosList.Count; index++)
            {
                string uniqueID = _heroDataList[index].UniqueID;
                Vector2 newPos = cachePosList[index];
                Vector3 newDir = cacheDirList[index];
                CharacterGameObject character = _heroGameObjectDict[uniqueID];
                
                character.SetCharacterPosition(newPos);
                character.SetCharacterDirection(newDir);
                onUpdateGrid?.Invoke(newPos, uniqueID);
            }
        }

        public void UpdateCharacterLevel(int level)
        {
            if (_heroDataList is null or {Count: 0})
                return;
            
            _heroDataList.ForEach(data => data.UpdateCharacterStat(level));
        }

        #endregion
    }
}
