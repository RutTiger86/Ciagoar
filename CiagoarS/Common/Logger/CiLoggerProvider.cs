using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System;
using System.Runtime.Versioning;

namespace CiagoarS.Common.Logger
{
    [ProviderAlias("CiagoarS")]
    public class CiLoggerProvider : ILoggerProvider
    {
        private readonly IDisposable _onChangeToken;
        private CiLoggerConfiguration _currentConfig;
        private readonly ConcurrentDictionary<string, CiLogger> _loggers =
            new(StringComparer.OrdinalIgnoreCase);

        public CiLoggerProvider(IOptionsMonitor<CiLoggerConfiguration> config)
        {
            _currentConfig = config.CurrentValue;
            _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
        }

        public ILogger CreateLogger(string categoryName) =>_loggers.GetOrAdd(categoryName, name => new CiLogger(name, GetCurrentConfig));

        private CiLoggerConfiguration GetCurrentConfig() => _currentConfig;

        public void Dispose()
        {
            _loggers.Clear();
            _onChangeToken.Dispose();
        }
    }
}
