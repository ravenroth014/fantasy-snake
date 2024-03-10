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
        public ECharacterType CharacterType { get; private set; }

        public bool IsDead => CurrentHp > 0;
        
        private readonly int _baseHpStat;
        private readonly int _baseAtkStat;
        private readonly float _hpGrowthRate;
        private readonly float _atkGrowthRate;
        private int _currentLevel;
        
        #endregion

        #region Constructors

        public CharacterData(CharacterBaseStat baseStat, int currentLevel)
        {
            _baseHpStat = baseStat.BaseHp;
            _baseAtkStat = baseStat.BaseHp;
            _hpGrowthRate = baseStat.HpGrowthRate;
            _atkGrowthRate = baseStat.AtkGrowthRate;
            _currentLevel = 0;

            UpdateCharacterStat(currentLevel);
        }

        #endregion
        
        #region Methods

        public void UpdateCharacterStat(int level)
        {
            if (level <= _currentLevel) return;
            
            MaxHp = GameHelper.CalculateCharacterStat(_baseHpStat, level, _hpGrowthRate);
            CurrentHp = MaxHp;

            AtkPoint = GameHelper.CalculateCharacterStat(_baseAtkStat, level, _atkGrowthRate);
            _currentLevel = level;
        }

        public void TakeDamage(int value)
        {
            CurrentHp -= value;
        }

        #endregion
    }
}
