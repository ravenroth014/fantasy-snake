using System.Collections.Generic;
using System.Linq;
using FS_Runtimes.Utilities;
using UnityEngine;

namespace FS_Runtimes.Models.Levels
{
    public class LevelModelManager
    {
        #region Fields & Properties

        private readonly int _horizontalMaxSize = 16;
        private readonly int _verticalMaxSize = 16;
        private readonly Dictionary<Vector2, GridData> _gridDict = new();

        #endregion

        #region Constructor

        public LevelModelManager()
        {
            GenerateDict();
        }

        #endregion
        
        #region Methods

        private void GenerateDict()
        {
            for (int hIndex = 1; hIndex <= _horizontalMaxSize; hIndex++)
            {
                for (int vIndex = 1; vIndex <= _verticalMaxSize; vIndex++)
                {
                    Vector2 newVector = new Vector2(hIndex, vIndex);
                    _gridDict.Add(newVector, new GridData(newVector));
                }
            }
        }
        
        public void Reset()
        {
            if (_gridDict is null or { Count: 0 })
                GenerateDict();
            else
                _gridDict.Values.ToList().ForEach(grid => grid.Reset());
        }

        public Vector2 GetFreePosition()
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
        
        #endregion
    }
}
