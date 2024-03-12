using FS_Runtimes.Controllers.Character;

namespace FS_Runtimes.Models.Characters
{
    public class CharacterPairData
    {
        #region Fields & Properties
        
        public CharacterData CharacterData;
        public CharacterGameObject CharacterGameObject;

        #endregion

        #region Constructors
        
        public CharacterPairData(CharacterData characterData,CharacterGameObject characterGameObject)
        {
            CharacterData = characterData;
            CharacterGameObject = characterGameObject;
        }
        
        #endregion
    }
}