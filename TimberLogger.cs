using NLog;
using Timber.io.NLog.Configuration;
using Timber.io.NLog.Http;

namespace Timber.io.NLog
{
    public class TimberLogger : Logger
    {
        private readonly string _category;
        private readonly TimberLoggerConfiguration _config;
        private readonly ITimberClient _timberClient;

        public TimberLogger(string category, TimberLoggerConfiguration config)
        {
            _category = category;
            _config = config;
            _timberClient = new TimberHttpClient(_config.TimberUrl, _config.ApiKey, config.Format);
        }

        public new void Log(LogLevel level, string message)
        {
            _timberClient.Send(level, _category, message);
        }
    }
}
