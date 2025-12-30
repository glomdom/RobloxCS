using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace RobloxCS.Common;

public static class LoggerSetup {
    public static readonly LoggingLevelSwitch LevelSwitch = new() { MinimumLevel = LogEventLevel.Warning };

    public static void Configure() {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(LevelSwitch)
            .WriteTo.Console(standardErrorFromLevel: LogEventLevel.Verbose)
            .CreateLogger();
    }
}