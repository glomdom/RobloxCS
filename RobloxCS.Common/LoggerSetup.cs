using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace RobloxCS.Common;

public static class LoggerSetup {
    public static readonly LoggingLevelSwitch LevelSwitch = new() { MinimumLevel = LogEventLevel.Warning };

    private const string Template = "[{Timestamp:HH:mm:ss} {Level:u3}]{PassContext} {Message:lj}{NewLine}{Exception}";

    public static void Configure() {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(LevelSwitch)
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: Template, standardErrorFromLevel: LogEventLevel.Verbose)
            .CreateLogger();
    }

    public static IDisposable PushPass(string passName) {
        return Serilog.Context.LogContext.PushProperty("PassContext", $" [{passName}]");
    }
}