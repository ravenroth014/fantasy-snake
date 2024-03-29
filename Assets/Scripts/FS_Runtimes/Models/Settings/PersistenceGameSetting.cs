using Newtonsoft.Json;

namespace FS_Runtimes.Models.Settings
{
    public class PersistenceGameSetting
    {
        #region Fields & Properties
        
        [JsonProperty(PropertyName = "StartEntity")]
        public int StartEntity { get; private set; }
        
        [JsonProperty(PropertyName = "MinAttack")]
        public int MinAttack { get; private set; }
        
        [JsonProperty(PropertyName = "MaxAttack")]
        public int MaxAttack { get; private set; }
        
        [JsonProperty(PropertyName = "MinHealth")]
        public int MinHealth { get; private set; }
        
        [JsonProperty(PropertyName = "MaxHealth")]
        public int MaxHealth { get; private set; }
        
        [JsonProperty(PropertyName = "BaseMoveLevelUp")]
        public int BaseMoveLevelUp { get; private set; }
        
        [JsonProperty(PropertyName = "StatGrowthRate")]
        public float StatGrowthRate { get; private set; }
        
        [JsonProperty(PropertyName = "MoveLevelUpGrowthRate")]
        public float MoveLevelUpGrowthRate { get; private set; }
        
        [JsonProperty(PropertyName = "MaxActiveEntity")]
        public int MaxActiveEntity { get; private set; }
        
        [JsonProperty(PropertyName = "MaxSpawnable")]
        public int MaxSpawnable { get; private set; }
        
        #endregion

        #region Constructors

        public PersistenceGameSetting()
        {
            StartEntity = 0;
            MinAttack = 0;
            MaxAttack = 0;
            MinHealth = 0;
            MaxHealth = 0;
            BaseMoveLevelUp = 0;
            StatGrowthRate = 0;
            MoveLevelUpGrowthRate = 0;
            MaxActiveEntity = 0;
            MaxSpawnable = 0;
        }
        
        public PersistenceGameSetting(int startEntity, int minAttack, int maxAttack, int minHealth, int maxHealth, int baseMoveLevelUp, float statGrowthRate, float moveLevelUpGrowthRate, int maxActiveEntity, int maxSpawnable)
        {
            StartEntity = startEntity;
            MinAttack = minAttack;
            MaxAttack = maxAttack;
            MinHealth = minHealth;
            MaxHealth = maxHealth;
            BaseMoveLevelUp = baseMoveLevelUp;
            StatGrowthRate = statGrowthRate;
            MoveLevelUpGrowthRate = moveLevelUpGrowthRate;
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
            BaseMoveLevelUp = data.BaseMoveLevelUp;
            StatGrowthRate = data.StatGrowthRate;
            MoveLevelUpGrowthRate = data.MoveLevelUpGrowthRate;
            MaxActiveEntity = data.MaxActiveEntity;
            MaxSpawnable = data.MaxSpawnable;
        }
        
        #endregion
    }
}
