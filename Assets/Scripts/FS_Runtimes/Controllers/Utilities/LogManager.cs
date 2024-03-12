using System;
using System.IO;
using System.Text;
using FS_Runtimes.Utilities.Setting;
using UnityEngine;

namespace FS_Runtimes.Controllers.Utilities
{
    public class LogManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Setting")] 
        [SerializeField, Tooltip("Log Setting")] private LogSetting _logSetting;

        public static LogManager Instance => _instance;
        private static LogManager _instance;

        private static readonly string LogFileNamePrefix = "Log";
        private static readonly string LogWarningFileNamePrefix = "LogWarning";
        private static readonly string LogErrorFileNamePrefix = "LogError";
        private static readonly string LogAssertionFileNamePrefix = "LogAssertion";

        private static readonly string MyGameFolderName = "My Games";

        private string _logFolderPath;
        private string _logWarningFolderPath;
        private string _logErrorFolderPath;
        private string _logAssertionFolderPath;
        
        private string _logFileName;
        private string _logWarningFileName;
        private string _logErrorFileName;
        private string _logAssertionFileName;

        private StringBuilder _logBuilder;
        private StringBuilder _logWarningBuilder;
        private StringBuilder _logErrorBuilder;
        private StringBuilder _logAssertionBuilder;

        #endregion

        #region Methods

        #region Unity Event Methods
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
                Init();
                return;
            }
            
            Destroy(this);
            Unsubscribe();
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void OnApplicationQuit()
        {
            WriteLogFile();
            WriteLogWarningFile();
            WriteLogErrorFile();
            WriteLogAssertionFile();
        }

        #endregion

        #region Init Methods
        
        /// <summary>
        /// Call this method to initialize log manager.
        /// </summary>
        private void Init()
        {
            InitFolder();
            InitFileName();
            InitStringBuilder();
        }

        /// <summary>
        /// Call this method to initialize folder.
        /// </summary>
        private void InitFolder()
        {
            string logPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/");
            
            logPath = $"{logPath}/{MyGameFolderName}";
            if (Directory.Exists(logPath) == false)
                CreateFolder(logPath);

            logPath = $"{logPath}/{Application.productName}";
            if (Directory.Exists(logPath) == false)
                CreateFolder(logPath);

            logPath = $"{logPath}/logs";
            if (Directory.Exists(logPath) == false)
                CreateFolder(logPath);

            InitLogFolder(logPath);
        }

        /// <summary>
        /// Call this method to initialize log folder.
        /// </summary>
        /// <param name="logPath"></param>
        private void InitLogFolder(string logPath)
        {
            _logFolderPath = $"{logPath}/log";
            _logWarningFolderPath = $"{logPath}/log_warning";
            _logErrorFolderPath = $"{logPath}/log_error";
            _logAssertionFolderPath = $"{logPath}/log_assertion";
            
            if (Directory.Exists(_logFolderPath) == false)
                CreateFolder(_logFolderPath);
            
            if (Directory.Exists(_logWarningFolderPath) == false)
                CreateFolder(_logWarningFolderPath);
            
            if (Directory.Exists(_logErrorFolderPath) == false)
                CreateFolder(_logErrorFolderPath);
            
            if (Directory.Exists(_logAssertionFolderPath) == false)
                CreateFolder(_logAssertionFolderPath);
        }

        /// <summary>
        /// Call this method to initialize file name.
        /// </summary>
        private void InitFileName()
        {
            DateTime currentTime = DateTime.Now;
            string time = currentTime.ToString("yyyy-MM-dd HH_mm_ss");
            
            if (_logSetting.IsLogMessage) 
                _logFileName = $"{LogFileNamePrefix}_{time}.txt";
            
            if (_logSetting.IsLogWarningMessage) 
                _logWarningFileName = $"{LogWarningFileNamePrefix}_{time}.txt";
            
            if (_logSetting.IsLogErrorMessage) 
                _logErrorFileName = $"{LogErrorFileNamePrefix}_{time}.txt";

            if (_logSetting.IsLogAssertionMessage)
                _logAssertionFileName = $"{LogAssertionFileNamePrefix}_{time}.txt";
        }

        /// <summary>
        /// Call this method to create folder.
        /// </summary>
        /// <param name="folderPath"></param>
        private void CreateFolder(string folderPath)
        {
            try
            {
                Directory.CreateDirectory(folderPath);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        /// <summary>
        /// Call this method to initialize string builder.
        /// </summary>
        private void InitStringBuilder()
        {
            _logBuilder = new StringBuilder();
            _logWarningBuilder = new StringBuilder();
            _logErrorBuilder = new StringBuilder();
            _logAssertionBuilder = new StringBuilder();
        }
        
        #endregion

        #region Log Methods

        /// <summary>
        /// Call this method to log normal message.
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            #if UNITY_EDITOR
                if (_logSetting.IsLogMessage)
                    Debug.Log(message);
            #else
                OnDebugLog(message, string.Empty, LogType.Log);
            #endif
        }

        /// <summary>
        /// Call this method to log warning message.
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning(string message)
        {
            #if UNITY_EDITOR
                if (_logSetting.IsLogWarningMessage)
                    Debug.LogWarning(message);
            #else
                OnDebugLog(message, string.Empty, LogType.Warning);
            #endif
        }

        /// <summary>
        /// Call this method to log error message.
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message)
        {
            #if UNITY_EDITOR
                if (_logSetting.IsLogErrorMessage)
                    Debug.LogError(message);
            #else
                OnDebugLog(message, string.Empty, LogType.Error);
            #endif
        }

        /// <summary>
        /// Call this method to log assertion message.
        /// </summary>
        /// <param name="message"></param>
        public void LogAssertion(string message)
        {
            #if UNITY_EDITOR
                if (_logSetting.IsLogAssertionMessage)
                    Debug.LogAssertion(message);
            #else
                OnDebugLog(message, string.Empty, LogType.Assert);
            #endif
        }
        
        #endregion

        #region Observer Methods
        
        private void Subscribe()
        {
            Application.logMessageReceived += OnDebugLog;
        }

        private void Unsubscribe()
        {
            Application.logMessageReceived -= OnDebugLog;
        }
        
        #endregion

        #region Event Methods

        /// <summary>
        /// Callback method when debug log event is triggered.
        /// </summary>
        /// <param name="logString"></param>
        /// <param name="stacktrace"></param>
        /// <param name="logType"></param>
        private void OnDebugLog(string logString, string stacktrace, LogType logType)
        {
            switch (logType)
            {
                case LogType.Error:
                    OnWriteLogErrorOnFile(logString, stacktrace);
                    break;
                case LogType.Assert:
                    OnWriteLogAssertionOnFile(logString, stacktrace);
                    break;
                case LogType.Warning:
                    OnWriteLogWarningOnFile(logString, stacktrace);
                    break;
                case LogType.Log:
                    OnWriteLogOnFile(logString, stacktrace);
                    break;
            }
        }

        private void OnWriteLogOnFile(string logString, string stacktrace)
        {
            if (_logSetting.IsLogMessage == false) return;
            
            string line = _logSetting.IsStacktraceEnable == false
                ? $"[{DateTime.Now:s}] : {logString}"
                : $"[{DateTime.Now:s}] : {logString}\n{stacktrace}";

            _logBuilder.AppendLine(line);
        }

        private void OnWriteLogWarningOnFile(string logString, string stacktrace)
        {
            if (_logSetting.IsLogWarningMessage == false) return;
            
            string line = _logSetting.IsStacktraceEnable == false
                ? $"[{DateTime.Now:s}] : {logString}"
                : $"[{DateTime.Now:s}] : {logString}\n{stacktrace}";

            _logWarningBuilder.AppendLine(line);
        }

        private void OnWriteLogErrorOnFile(string logString, string stacktrace)
        {
            if (_logSetting.IsLogErrorMessage == false) return;

            string line = _logSetting.IsStacktraceEnable == false
                ? $"[{DateTime.Now:s}] : {logString}"
                : $"[{DateTime.Now:s}] : {logString}\n{stacktrace}";

            _logErrorBuilder.AppendLine(line);
        }

        private void OnWriteLogAssertionOnFile(string logString, string stacktrace)
        {
            if (_logSetting.IsLogAssertionMessage == false) return;
            
            string line = _logSetting.IsStacktraceEnable == false
                ? $"[{DateTime.Now:s}] : {logString}"
                : $"[{DateTime.Now:s}] : {logString}\n{stacktrace}";

            _logAssertionBuilder.AppendLine(line);
        }
        
        #endregion

        #region Write Files Methods

        /// <summary>
        /// Call this method to write normal log file.
        /// </summary>
        private void WriteLogFile()
        {
            if (_logSetting.IsLogMessage == false) return;
            if (string.IsNullOrEmpty(_logBuilder.ToString())) return;
            
            TextWriter tw = new StreamWriter(Path.Combine(_logFolderPath, _logFileName), true);
            tw.WriteLine(_logBuilder.ToString());
            tw.Close();
        }

        /// <summary>
        /// Call this method to write warning log file.
        /// </summary>
        private void WriteLogWarningFile()
        {
            if (_logSetting.IsLogWarningMessage == false) return;
            if (string.IsNullOrEmpty(_logWarningBuilder.ToString())) return;
            
            TextWriter tw = new StreamWriter(Path.Combine(_logWarningFolderPath, _logWarningFileName), true);
            tw.WriteLine(_logWarningBuilder.ToString());
            tw.Close();
        }

        /// <summary>
        /// Call this method to write error log file.
        /// </summary>
        private void WriteLogErrorFile()
        {
            if (_logSetting.IsLogErrorMessage == false) return;
            if (string.IsNullOrEmpty(_logErrorBuilder.ToString())) return;
            
            TextWriter tw = new StreamWriter(Path.Combine(_logErrorFolderPath, _logErrorFileName), true);
            tw.WriteLine(_logErrorBuilder.ToString());
            tw.Close();
        }

        /// <summary>
        /// Call this method to write assertion log file.
        /// </summary>
        private void WriteLogAssertionFile()
        {
            if (_logSetting.IsLogAssertionMessage == false) return;
            if (string.IsNullOrEmpty(_logAssertionBuilder.ToString())) return;
            
            TextWriter tw = new StreamWriter(Path.Combine(_logAssertionFolderPath, _logAssertionFileName), true);
            tw.WriteLine(_logAssertionBuilder.ToString());
            tw.Close();
        }

        #endregion
        
        #endregion
    }
}
