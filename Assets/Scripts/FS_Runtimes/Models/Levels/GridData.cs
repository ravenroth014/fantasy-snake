using FS_Runtimes.Controllers.Characters;
using FS_Runtimes.Utilities;

namespace FS_Runtimes.Models.Levels
{
    public class GridData
    {
        #region Fields & Properties

        public int HorizontalIndex;
        public int VerticalIndex;
        
        public CharacterController CurrentCharacter;
        public EGridState GridState;

        #endregion
    }
}
