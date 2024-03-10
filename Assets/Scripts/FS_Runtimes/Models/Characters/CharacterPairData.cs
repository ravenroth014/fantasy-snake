using FS_Runtimes.Controllers.Character;

namespace FS_Runtimes.Models.Characters
{
    public class CharacterPairData
    {
        public CharacterData CharacterData;
        public CharacterGameObject CharacterGameObject;

        public CharacterPairData(CharacterData characterData,CharacterGameObject characterGameObject)
        {
            CharacterData = characterData;
            CharacterGameObject = characterGameObject;
        }
    }
}