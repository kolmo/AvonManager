using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

namespace AvonManager.Common.Helpers
{
    public class Logger
    {
        public const string CATEGORY = "General";
        private const int _defaultPriority = -1;
        private static Logger _instance;
        private LogWriter _logWriter;
        private Logger()
        {
            SetUpLogger();
        }

        private void SetUpLogger()
        {
            LogWriterFactory factory = new LogWriterFactory();
            _logWriter = factory.Create();
        }

        public static Logger Current
        {
            get { return _instance ?? (_instance = new Logger()); }
        }
        public void Write(object message, string category = CATEGORY, int priority = _defaultPriority, TraceEventType severity = TraceEventType.Information)
        {
            _logWriter.Write(message, category, priority, 10000, severity);
        }
    }
}
