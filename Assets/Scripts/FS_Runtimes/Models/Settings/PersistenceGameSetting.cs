namespace FS_Runtimes.Models.Settings
{
    public class PersistenceGameSetting
    {
        #region Fields & Properties
        
        public int StartEntity { get; private set; }
        public int MinAttack { get; private set; }
        public int MaxAttack { get; private set; }
        public int MinHealth { get; private set; }
        public int MaxHealth { get; private set; }
        public int GrowthWithMove { get; private set; }
        public int MaxActiveEntity { get; private set; }
        public int MaxSpawnable { get; private set; }
        
        #endregion

        #region Constructors

        public PersistenceGameSetting(int startEntity, int minAttack, int maxAttack, int minHealth, int maxHealth, int growthWithMove, int maxActiveEntity, int maxSpawnable)
        {
            StartEntity = startEntity;
            MinAttack = minAttack;
            MaxAttack = maxAttack;
            MinHealth = minHealth;
            MaxHealth = maxHealth;
            GrowthWithMove = growthWithMove;
            MaxActiveEntity = maxActiveEntity;
            MaxSpawnable = maxSpawnable;
        }

        public PersistenceGameSetting(PersistenceGameSetting data)
        {
            StartEntity = data.StartEntity;
            MinAttack = data.MinAttack;
            MaxAttack = data.MaxAttack;
            MinHealth = data.MinHealth;
            MaxHealth = data.MaxHealth;
            GrowthWithMove = data.GrowthWithMove;
            MaxActiveEntity = data.MaxActiveEntity;
            MaxSpawnable = data.MaxSpawnable;
        }
        
        #endregion
    }
}
