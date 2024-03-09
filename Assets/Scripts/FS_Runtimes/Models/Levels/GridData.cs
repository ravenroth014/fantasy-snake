using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.Models.Levels
{
    public class GridData
    {
        #region Fields & Properties

        public Vector2 GridIndex { get; private set; }
        public string CharacterUniqueID { get; private set; }
        public EGridState GridState { get; private set; }
        public ECharacterType CharacterType { get; private set; }

        #endregion

        #region Constructors

        public GridData(Vector2 gridIndex, EGridState gridState)
        {
            GridIndex = gridIndex;
            CharacterUniqueID = string.Empty;
            GridState = gridState;
            CharacterType = ECharacterType.None;
        }

        #endregion
        
        #region Methods

        public void UpdateData(string uniqueID, EGridState gridState, ECharacterType characterType)
        {
            CharacterUniqueID = uniqueID;
            GridState = gridState;
            CharacterType = characterType;
        }
        
        public void Reset()
        {
            CharacterUniqueID = string.Empty;
            GridState = EGridState.Empty;
        }

        #endregion
    }
}
