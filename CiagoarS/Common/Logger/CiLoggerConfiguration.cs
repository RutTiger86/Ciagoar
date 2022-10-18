using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;

namespace CiagoarS.Common.Logger
{
    public class CiLoggerConfiguration
    {
        public int EventId { get; set; }

        public Dictionary<LogLevel, ConsoleColor> LogLevelToColorMap { get; set; } = new()
        {
            [LogLevel.Information] = ConsoleColor.Green
        };
    }
}
