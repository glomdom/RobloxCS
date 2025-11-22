using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace RobloxCS.Common;

public class Logger {
    public static readonly LoggingLevelSwitch LevelSwitch = new() { MinimumLevel = LogEventLevel.Warning };

    static Logger() {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(LevelSwitch)
            .WriteTo.Console(standardErrorFromLevel: LogEventLevel.Verbose)
            .CreateLogger();
    }

    public static object[] Args(params object[] args) => args;

    [Conditional("DEBUG")]
    [SuppressMessage("Usage", "CA2254")]
    public static void Debug(string messageTemplate, params object?[] args) {
        Log.Debug(messageTemplate, args);
    }
    
    [Conditional("DEBUG")]
    [SuppressMessage("Usage", "CA2254")]
    public static void Verbose(string messageTemplate, params object?[] args) {
        Log.Verbose(messageTemplate, args);
    }
}