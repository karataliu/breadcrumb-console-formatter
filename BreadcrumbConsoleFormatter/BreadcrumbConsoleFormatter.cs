using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace Util.Extensions.Logging
{
    public class BreadcrumbConsoleFormatter : ConsoleFormatter
    {
        private static readonly BreadcrumbScope empty = new();
        private readonly BreadcrumbConsoleFormatterOptions bcfo;
        public BreadcrumbConsoleFormatter(IOptions<BreadcrumbConsoleFormatterOptions> jo)
            : base(nameof(BreadcrumbConsoleFormatter))
        {
            this.bcfo = jo.Value;
        }

        public override void Write<TState>(
            in LogEntry<TState> logEntry,
            IExternalScopeProvider scopeProvider,
            TextWriter textWriter)
        {
            var dic = new Dictionary<string, string>(bcfo.StaticScope ?? empty)
            {
                { "time"        , DateTime.UtcNow.ToString("s") },
                { "level"       , logEntry.LogLevel switch
                                    {
                                        LogLevel.Information => "info",
                                        _ => logEntry.LogLevel.ToString().ToLowerInvariant()
                                    } },
                { "category"    , logEntry.Category },
                { "msg"         , logEntry.Formatter!(logEntry.State, logEntry.Exception) },
            };
            scopeProvider.ForEachScope((scope, state) =>
            {
                if (scope is not BreadcrumbScope bc) return;
                foreach (var item in bc) dic[item.Key] = item.Value;
            }, dic);
            textWriter.WriteLine(!bcfo.Simple ? SerializeJson(dic) : SerializeText(dic));
        }

        public virtual string SerializeJson(in IReadOnlyDictionary<string, string> dic) => JsonSerializer.Serialize(dic);

        public virtual string SerializeText(in IReadOnlyDictionary<string, string> dic)
        {
            var stringBuilder = new StringBuilder();
            bool first = true;
            foreach (var item in dic)
            {
                if (first) first = false;
                else stringBuilder.Append(' ');
                stringBuilder.AppendFormat("{0}={1}", item.Key, item.Value);
            }
            return stringBuilder.ToString();
        }
    }
}
