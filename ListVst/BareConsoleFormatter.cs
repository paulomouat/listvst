using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace ListVst;

public class BareConsoleFormatter : ConsoleFormatter
{
    private readonly IDisposable _optionsReloadToken;
    private ConsoleFormatterOptions _formatterOptions;
    
    public BareConsoleFormatter(IOptionsMonitor<ConsoleFormatterOptions> options)
        : base("bare") =>
            (_optionsReloadToken, _formatterOptions) =
            (options.OnChange(ReloadLoggerOptions), options.CurrentValue);
    
    private void ReloadLoggerOptions(ConsoleFormatterOptions options) =>
        _formatterOptions = options;
    
    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter)
    {
        var message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);
        if (message is null)
        {
            return;
        }

        textWriter.Write(message);
        textWriter.Write(Environment.NewLine);
        
        var exception = logEntry.Exception;
        if (exception is not null)
        {
            textWriter.Write(exception.ToString());
            textWriter.Write(Environment.NewLine);
        }
    }
}