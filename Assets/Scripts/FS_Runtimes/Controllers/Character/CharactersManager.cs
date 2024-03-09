using System.Collections.Generic;
using System.Linq;
using System.Text;
using FS_Runtimes.Controllers.Core;
using FS_Runtimes.Models.Characters;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.Controllers.Character
{
    public class CharactersManager : MonoBehaviour
    {
        #region Fields & Properties

        private readonly Dictionary<string, CharacterData> _heroDataDict = new();
        private readonly Dictionary<string, CharacterData> _enemyDataDict = new();

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

        public string AddCharacter(CharacterGameObject characterGameObject, ECharacterType characterType)
        {
            _stringBuilder ??= new StringBuilder();
            _stringBuilder.Clear();

            string uniqueID = string.Empty;
            
            // TODO: Generate character data base on stage level.

            if (characterType == ECharacterType.Enemy)
            {
                _stringBuilder.Append(_enemyPrefix);
                _stringBuilder.Append(_currentEnemyUniqueID.ToString("0000"));
                _currentEnemyUniqueID++;
                
                uniqueID = _stringBuilder.ToString();

                _enemyGameObjectDict[uniqueID] = characterGameObject;
            }
            else if (characterType == ECharacterType.Hero)
            {
                _stringBuilder.Append(_heroPrefix);
                _stringBuilder.Append(_currentHeroUniqueID.ToString("0000"));
                _currentHeroUniqueID++;
                
                uniqueID = _stringBuilder.ToString();

                _heroGameObjectDict[uniqueID] = characterGameObject;
            }

            return uniqueID;
        }

        public void ResetManager()
        {
            _currentHeroUniqueID = 0;
            _currentEnemyUniqueID = 0;
            
            _heroDataDict.Clear();
            _enemyDataDict.Clear();

            if (_heroGameObjectDict is { Count: > 0 })
            {
                GameplayManager.Instance.ReturnItemListToPool(_heroGameObjectDict.Values.ToList(), ECharacterType.Hero);
                _heroGameObjectDict.Clear();
            }

            if (_enemyGameObjectDict is { Count: > 0 })
            {
                GameplayManager.Instance.ReturnItemListToPool(_enemyGameObjectDict.Values.ToList(), ECharacterType.Enemy);
            }
        }

        #endregion
    }
}
