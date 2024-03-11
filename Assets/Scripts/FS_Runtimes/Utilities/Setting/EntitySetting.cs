using UnityEngine;

namespace FS_Runtimes.Utilities.Setting
{
    [CreateAssetMenu(fileName = "EntitySetting", menuName = "Settings/Entity Setting")]
    public class EntitySetting : ScriptableObject
    {
        #region Fields & Properites

        [SerializeField, Tooltip("Default Start Entity")] private int _defaultStartEntity = 3;
        [SerializeField, Tooltip("Default Max Active Entity")] private int _defaultMaxActiveEntity = 5;
        [SerializeField, Tooltip("Default Max Spawnable")] private int _defaultMaxSpawnable = 3;

        public int DefaultStartEntity => _defaultStartEntity;
        public int DefaultMaxActiveEntity => _defaultMaxActiveEntity;
        public int DefaultMaxSpawnable => _defaultMaxSpawnable;

        #endregion
    }
}
