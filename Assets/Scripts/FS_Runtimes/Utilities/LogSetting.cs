using UnityEngine;

namespace FS_Runtimes.Utilities
{
    [CreateAssetMenu(fileName = "LogSetting", menuName = "Settings/Log Setting")]
    public class LogSetting : ScriptableObject
    {
        #region Fields & Properties

        [Header("Log Setting")] 
        [SerializeField, Tooltip("Collect Log Message")] private bool _isLogMessage = true;
        [SerializeField, Tooltip("Collect Log Warning Message")] private bool _isLogWarningMessage = true;
        [SerializeField, Tooltip("Collect Log Error Message")] private bool _isLogErrorMessage = true;
        [SerializeField, Tooltip("Collect Log Assertion Message")] private bool _isLogAssertionMessage = true;
        [SerializeField, Tooltip("Stacktrace enable")] private bool _isStacktraceEnable = true;

        public bool IsLogMessage => _isLogMessage;
        public bool IsLogWarningMessage => _isLogWarningMessage;
        public bool IsLogErrorMessage => _isLogErrorMessage;
        public bool IsLogAssertionMessage => _isLogAssertionMessage;
        public bool IsStacktraceEnable => _isStacktraceEnable;
        
        #endregion
    }
}
