using UnityEngine;

namespace FS_Runtimes.Utilities.Setting
{
    [CreateAssetMenu(fileName = "StatSetting", menuName = "Settings/Stat Setting")]
    public class StatSetting : ScriptableObject
    {
        #region Fields & Properties

        [SerializeField, Tooltip("Default Min Attack")] private int _defaultMinAttack = 10;
        [SerializeField, Tooltip("Default Max Attack")] private int _defaultMaxAttack = 100;
        [SerializeField, Tooltip("Default Min Health")] private int _defaultMinHealth = 50;
        [SerializeField, Tooltip("Default Max Health")] private int _defaultMaxHealth = 500;

        public int DefaultMinAttack => _defaultMinAttack;
        public int DefaultMaxAttack => _defaultMaxAttack;
        public int DefaultMinHealth => _defaultMinHealth;
        public int DefaultMaxHealth => _defaultMaxHealth;

        #endregion
    }
}
