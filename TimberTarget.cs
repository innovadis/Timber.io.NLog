using System.Collections.Concurrent;
using NLog;
using NLog.Config;
using NLog.Targets;
using Timber.io.NLog.Configuration;

namespace Timber.io.NLog
{
    [Target("Timber.io")]
    public class TimberTarget : TargetWithLayout
    {
        private readonly ConcurrentDictionary<string, TimberLogger> _loggers = new ConcurrentDictionary<string, TimberLogger>();
        
        [RequiredParameter]
        public string Token { get; set; }

        protected override void Write(LogEventInfo logEvent)
        {
            var logger = CreateLogger(logEvent.LoggerName);

            logger.Log(logEvent.Level, logEvent.Message);
        }


        public TimberLogger CreateLogger(string categoryName)
        {
            var configuration = new TimberLoggerConfiguration
            {
                ApiKey = Token
            };

            return _loggers.GetOrAdd(categoryName, name => new TimberLogger(name, configuration));
        }
    }
}
