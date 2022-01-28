using System.Collections.Concurrent;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace WebStore.Logging;

public class Log4NetLoggerProvider : ILoggerProvider
{
    private readonly string _ConfigurationFile;
    private readonly ConcurrentDictionary<string, Log4NetLogger> _Loggers = new();

    public Log4NetLoggerProvider(string ConfigurationFile) => _ConfigurationFile = ConfigurationFile;

    public ILogger CreateLogger(string Category) =>
        _Loggers.GetOrAdd(Category, static (category, config_file) =>
        {
            var xml = new XmlDocument();
            xml.Load(config_file);
            return new Log4NetLogger(category, xml["log4net"]!);
        }, _ConfigurationFile);

    public void Dispose() => _Loggers.Clear();
}
