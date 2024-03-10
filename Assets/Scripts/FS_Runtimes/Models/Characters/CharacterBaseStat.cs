using UnityEngine;

namespace FS_Runtimes.Models.Characters
{
    [CreateAssetMenu(fileName = "CharacterBaseStat", menuName = "Characters/Character Base Stat")]
    public class CharacterBaseStat : ScriptableObject
    {
        #region Fields & Properties

        [SerializeField, Tooltip("Base Attack Damage")] private int _baseAtk = 10;
        [SerializeField, Tooltip("Base Health Point")] private int _baseHp = 50;
        [SerializeField, Tooltip("Attack Growth Rate"), Range(0, 1)] private float _atkGrowthRate = 0.2f;
        [SerializeField, Tooltip("Health Growth Rate"), Range(0, 1)] private float _hpGrowthRate = 0.2f;

        private int BaseAtk => _baseAtk;
        private int BaseHp => _baseHp;
        private float AtkGrowthRate => _atkGrowthRate;
        private float HpGrowthRate => _hpGrowthRate;

        #endregion
    }
}
