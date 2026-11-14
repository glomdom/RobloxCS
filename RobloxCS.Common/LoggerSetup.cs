using Serilog;
using Serilog.Core;
using Serilog.Events;
using Spectre.Console;

namespace RobloxCS.Common;

public static class LoggerSetup {
    public static readonly LoggingLevelSwitch LevelSwitch = new() { MinimumLevel = LogEventLevel.Warning };

    public static bool IsVerbose => LevelSwitch.MinimumLevel <= LogEventLevel.Debug;

    public static void Configure(IAnsiConsole? console = null) {
        console ??= AnsiConsole.Create(new AnsiConsoleSettings { Out = new AnsiConsoleOutput(Console.Error) });

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(LevelSwitch)
            .Enrich.FromLogContext()
            .WriteTo.Sink(new TerminalOutput(console))
            .CreateLogger();
    }

    public static void ApplyVerbosity(bool apply) {
        LevelSwitch.MinimumLevel = apply ? LogEventLevel.Verbose : LogEventLevel.Warning;
    }

    public static IDisposable PushPass(string passName) {
        return Serilog.Context.LogContext.PushProperty("PassContext", passName);
    }
}