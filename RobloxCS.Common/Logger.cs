using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace RobloxCS.Common;

public class Logger {
    public static readonly LoggingLevelSwitch LevelSwitch = new() { MinimumLevel = LogEventLevel.Warning };

    static Logger() {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(LevelSwitch)
            .WriteTo.Console()
            .CreateLogger();
    }

    public static object[] Args(params object[] args) => args;
}