using FS_Runtimes.Utilities;

namespace FS_Runtimes.Models.Characters
{
    public class CharacterData
    {
        #region Fields & Properties
        
        public int MaxHp { get; private set; }
        public int CurrentHp { get; private set; }
        public int AtkPoint { get; private set; }
        public string UniqueID { get; private set; }
        public bool IsDead => CurrentHp <= 0;
        
        private readonly int _baseHpStat;
        private readonly int _baseAtkStat;
        private readonly float _hpGrowthRate;
        private readonly float _atkGrowthRate;
        private readonly int _growthByMoveRule;
        private int _currentLevel;
        private int _growthMoveLeft;
        
        #endregion

        #region Constructors

        public CharacterData(int baseAtkStat, int baseHpStat, float growingRate, int growthByMoveRule, string uniqueID)
        {
            _baseAtkStat = baseAtkStat;
            _baseHpStat = baseHpStat;
            _hpGrowthRate = growingRate;
            _atkGrowthRate = growingRate;
            _growthByMoveRule = growthByMoveRule;
            _growthMoveLeft = growthByMoveRule;
            _currentLevel = 1;

            UniqueID = uniqueID;
            
            UpdateCharacterStat();
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Call this method to update character stat.
        /// </summary>
        private void UpdateCharacterStat()
        {
            MaxHp = GameHelper.CalculateCharacterStat(_baseHpStat, _currentLevel, _hpGrowthRate);
            CurrentHp = MaxHp;

            AtkPoint = GameHelper.CalculateCharacterStat(_baseAtkStat, _currentLevel, _atkGrowthRate);
        }

        /// <summary>
        /// Call this method to deduct character health from damage.
        /// </summary>
        /// <param name="value"></param>
        public void TakeDamage(int value)
        {
            CurrentHp -= value;

            if (CurrentHp < 0)
                CurrentHp = 0;
        }

        /// <summary>
        /// Call this method to execute end turn action to check update stat condition.
        /// </summary>
        public void OnTakeAction()
        {
            _growthMoveLeft--;

            if (_growthMoveLeft <= 0)
            {
                _growthMoveLeft = _growthByMoveRule;
                _currentLevel++;
                UpdateCharacterStat();
            }
        }

        #endregion
    }
}
