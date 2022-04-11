using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HACC.Logging;

public sealed class LoggingProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, CustomLogger> _loggers = new();
    private readonly IDisposable _onChangeToken;
    private LoggingConfiguration _currentConfig;

    public LoggingProvider(
        IOptionsMonitor<LoggingConfiguration> config)
    {
        this._currentConfig = config.CurrentValue;
        this._onChangeToken = config.OnChange(listener: updatedConfig => this._currentConfig = updatedConfig);
    }

    public ILogger CreateLogger(string categoryName)
    {
        return this._loggers.GetOrAdd(key: categoryName,
            valueFactory: name => new CustomLogger(name: name,
                getCurrentConfig: this.GetCurrentConfig));
    }

    public void Dispose()
    {
        this._loggers.Clear();
        this._onChangeToken.Dispose();
    }

    private LoggingConfiguration GetCurrentConfig()
    {
        return this._currentConfig;
    }
}