using FS_Runtimes.Models.Characters;
using TMPro;
using UnityEngine;

namespace FS_Runtimes.Controllers.UI
{
    public class GameplayUIController : AbstractUIController
    {
        #region Fields & Properties

        [Header("Text UI")] 
        [SerializeField, Tooltip("Hero Health Text")] private TextMeshProUGUI _heroHealthText;
        [SerializeField, Tooltip("Hero Attack Text")] private TextMeshProUGUI _heroAttackText;
        [SerializeField, Tooltip("Enemy Health Text")] private TextMeshProUGUI _enemyHealthText;
        [SerializeField, Tooltip("Enemy Attack Text")] private TextMeshProUGUI _enemyAttackText;
        [SerializeField, Tooltip("Kill Count Text")] private TextMeshProUGUI _killCountText;

        #endregion

        #region Methods

        public void UpdatePlayerText(CharacterPairData heroData)
        {
            if (_heroHealthText is null || _heroAttackText is null)
                return;

            if (heroData == null)
            {
                _heroHealthText.text = "N/A";
                _heroAttackText.text = "N/A";
                return;
            }
            
            string healthString = heroData.CharacterData.CurrentHp.ToString("D");
            string maxHealthString = heroData.CharacterData.MaxHp.ToString("D");
            string attackString = heroData.CharacterData.AtkPoint.ToString("D");

            _heroHealthText.text = $"{healthString}/{maxHealthString}";
            _heroAttackText.text = attackString;
        }
        
        public void UpdateEnemyText(CharacterPairData enemyData)
        {
            if (_enemyHealthText is null || _enemyAttackText is null)
                return;

            if (enemyData == null)
            {
                _enemyHealthText.text = "N/A";
                _enemyAttackText.text = "N/A";
                return;
            }
            
            string healthString = enemyData.CharacterData.CurrentHp.ToString("D");
            string maxHealthString = enemyData.CharacterData.MaxHp.ToString("D");
            string attackString = enemyData.CharacterData.AtkPoint.ToString("D");

            _enemyHealthText.text = $"{healthString}/{maxHealthString}";
            _enemyAttackText.text = attackString;
        }

        public void UpdateKillCountText(int killCount)
        {
            if (_killCountText is not null)
                _killCountText.text = killCount.ToString("D");
        }

        #endregion
    }
}
