using AssetStudio;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetStudioGUI
{
    class GUILogger : ILogger
    {
        public static bool ShowDebugMessage = false;

        private bool isFileLoggerRunning = false;
        private string loggerInitString;
        private string fileLogName;
        private string fileLogPath;
        private Action<string> action;
        private CancellationTokenSource tokenSource;
        private BlockingCollection<string> consoleLogMessageCollection = new BlockingCollection<string>();
        private BlockingCollection<string> fileLogMessageCollection = new BlockingCollection<string>();

        private bool _useFileLogger = false;
        public bool UseFileLogger
        {
            get => _useFileLogger;
            set
            {
                _useFileLogger = value;
                if (_useFileLogger && !isFileLoggerRunning)
                {
                    var appAssembly = typeof(Program).Assembly.GetName();
                    fileLogName = $"{appAssembly.Name}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log";
                    fileLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileLogName);
                    tokenSource = new CancellationTokenSource();
                    isFileLoggerRunning = true;

                    ConcurrentFileWriter(tokenSource.Token);
                    LogToFile(LoggerEvent.Verbose, $"# {loggerInitString} - Logger launched #");
                }
                else if (!_useFileLogger && isFileLoggerRunning)
                {
                    LogToFile(LoggerEvent.Verbose, "# Logger closed #");
                    isFileLoggerRunning = false;
                    tokenSource.Cancel();
                    tokenSource.Dispose();
                }
            }
        }

        public GUILogger(Action<string> action)
        {
            this.action = action;

            var appAssembly = typeof(Program).Assembly.GetName();
            var arch = Environment.Is64BitProcess ? "x64" : "x32";
            var frameworkName = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
            loggerInitString = $"{appAssembly.Name} v{appAssembly.Version} [{arch}] [{frameworkName}]";
            try
            {
                Console.Title = $"Console Logger - {appAssembly.Name} v{appAssembly.Version}";
                Console.OutputEncoding = System.Text.Encoding.UTF8;
            }
            catch
            {
                // ignored
            }

            ConcurrentConsoleWriter();
            Console.WriteLine($"# {loggerInitString}");
        }

        private static string ColorLogLevel(LoggerEvent logLevel)
        {
            var formattedLevel = $"[{logLevel}]";
            switch (logLevel)
            {
                case LoggerEvent.Info:
                    return $"{formattedLevel.Color(ColorConsole.BrightCyan)}";
                case LoggerEvent.Warning:
                    return $"{formattedLevel.Color(ColorConsole.BrightYellow)}";
                case LoggerEvent.Error:
                    return $"{formattedLevel.Color(ColorConsole.BrightRed)}";
                default:
                    return formattedLevel;
            }
        }

        private static string FormatMessage(LoggerEvent logMsgLevel, string message, bool toConsole)
        {
            message = message.TrimEnd();
            var multiLine = message.Contains("\n");

            string formattedMessage;
            if (toConsole)
            {
                var colorLogLevel = ColorLogLevel(logMsgLevel);
                formattedMessage = $"{colorLogLevel} {message}";
                if (multiLine)
                {
                    formattedMessage = formattedMessage.Replace("\n", $"\n{colorLogLevel} ") + $"\n{colorLogLevel}";
                }
            }
            else
            {
                var curTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                message = Regex.Replace(message, @"\e\[[0-9;]*m(?:\e\[K)?", "");  //Delete ANSI colors
                var logLevel = $"{logMsgLevel.ToString().ToUpper(),-7}";
                formattedMessage = $"{curTime} | {logLevel} | {message}";
                if (multiLine)
                {
                    formattedMessage = formattedMessage.Replace("\n", $"\n{curTime} | {logLevel} | ") + $"\n{curTime} | {logLevel} |";
                }
            }
            return formattedMessage;
        }

        private void ConcurrentFileWriter(CancellationToken token)
        {
            Task.Run(() =>
            {
                using (var sw = new StreamWriter(fileLogPath, append: true, System.Text.Encoding.UTF8))
                {
                    sw.AutoFlush = true;
                    foreach (var msg in fileLogMessageCollection.GetConsumingEnumerable())
                    {
                        sw.WriteLine(msg);
                        if (token.IsCancellationRequested)
                            break;
                    }
                }
            }, token);
        }

        private void ConcurrentConsoleWriter()
        {
            Task.Run(() =>
            {
                foreach (var msg in consoleLogMessageCollection.GetConsumingEnumerable())
                {
                    Console.WriteLine(msg);
                }
            });
        }

        private void LogToFile(LoggerEvent logMsgLevel, string message)
        {
            fileLogMessageCollection.Add(FormatMessage(logMsgLevel, message, toConsole: false));
        }

        private void LogToConsole(LoggerEvent logMsgLevel, string message)
        {
            consoleLogMessageCollection.Add(FormatMessage(logMsgLevel, message, toConsole: true));
        }

        public void Log(LoggerEvent loggerEvent, string message, bool ignoreLevel)
        {
            //File logger
            if (_useFileLogger)
            {
                LogToFile(loggerEvent, message);
            }

            //Console logger
            if (!ShowDebugMessage && loggerEvent == LoggerEvent.Debug)
                return;
            LogToConsole(loggerEvent, message);

            //GUI logger
            switch (loggerEvent)
            {
                case LoggerEvent.Error:
                    MessageBox.Show(message, "Error");
                    break;
                case LoggerEvent.Warning:
                    action("Some warnings occurred. See Console Logger for details.");
                    break;
                case LoggerEvent.Debug:
                    break;
                default:
                    action(message);
                    break;
            }
        }
    }
}
