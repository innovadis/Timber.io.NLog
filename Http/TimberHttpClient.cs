using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;

namespace Timber.io.NLog.Http
{
    public interface ITimberClient
    {
        Task Send(string log);

        Task Send(LogLevel logLevel, string category, string message);
    }

    public class TimberHttpClient : ITimberClient
    {
        private readonly string _timberUrl;
        private readonly string _timberToken;
        private readonly string _format;

        private const string TimberSchema =
            "https://raw.githubusercontent.com/timberio/log-event-json-schema/v3.1.3/schema.json";

        private const string HttpPostPath = "/frames";

        public TimberHttpClient(string timberUrl, string timberToken, string format)
        {
            _timberUrl = timberUrl;
            _timberToken = timberToken;
            _format = format;
        }

        public async Task Send(string log)
        {
            await PostLog(new StringContent(log));
        }

        public async Task Send(LogLevel logLevel, string category, string message)
        {
            try
            {
                var log = new ExpandoObject() as IDictionary<string, object>;
                log["$schema"] = TimberSchema;
                log["dt"] = DateTime.UtcNow;
                log["level"] = GetLevel(logLevel);
                log["message"] = String.Format(_format, category, message);

                var jsonLog = JsonConvert.SerializeObject(new object[] { log });

                await PostLog(new StringContent(jsonLog, Encoding.UTF8, "application/json"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task PostLog(StringContent content)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_timberUrl);

                    var byteArray = Encoding.ASCII.GetBytes(_timberToken);
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    var result = await client.PostAsync(HttpPostPath, content);
                    await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string GetLevel(LogLevel level)
        {
            if (level == LogLevel.Fatal) return "critical";
            if (level == LogLevel.Debug) return "debug";
            if (level == LogLevel.Error) return "error";
            if (level == LogLevel.Info) return "info";
            if (level == LogLevel.Trace) return "debug";
            if (level == LogLevel.Warn) return "warn";

            return "alert";
        }
    }
}
