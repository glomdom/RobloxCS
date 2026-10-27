using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace RobloxCS.Common;

public static class LoggerSetup {
    public static readonly LoggingLevelSwitch LevelSwitch = new() { MinimumLevel = LogEventLevel.Error };

    private static readonly AnsiConsoleTheme Theme = new(new Dictionary<ConsoleThemeStyle, string> {
        [ConsoleThemeStyle.Text] = "\e[0m",
        [ConsoleThemeStyle.SecondaryText] = "\e[90m",
        [ConsoleThemeStyle.TertiaryText] = "\e[90m",
        [ConsoleThemeStyle.Invalid] = "\e[33m",
        [ConsoleThemeStyle.Null] = "\e[34m",
        [ConsoleThemeStyle.Name] = "\e[37m",
        [ConsoleThemeStyle.String] = "\e[36m",
        [ConsoleThemeStyle.Number] = "\e[35m",
        [ConsoleThemeStyle.Boolean] = "\e[34m",
        [ConsoleThemeStyle.Scalar] = "\e[32m",
        [ConsoleThemeStyle.LevelVerbose] = "\e[90m",
        [ConsoleThemeStyle.LevelDebug] = "\e[37m",
        [ConsoleThemeStyle.LevelInformation] = "\e[34m",
        [ConsoleThemeStyle.LevelWarning] = "\e[33m",
        [ConsoleThemeStyle.LevelError] = "\e[31m",
        [ConsoleThemeStyle.LevelFatal] = "\e[97;41m",
    });
    
    private const string Template = "{Timestamp:HH:mm:ss}  {Level:u3}{PassContext} {Message:lj}{NewLine}{Exception}";

    public static void Configure() {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(LevelSwitch)
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: Template, theme: Theme, standardErrorFromLevel: LogEventLevel.Verbose)
            .CreateLogger();
    }

    public static void ApplyVerbosity(bool apply) {
        LevelSwitch.MinimumLevel = apply ? LogEventLevel.Verbose : LogEventLevel.Error;
    }

    public static IDisposable PushPass(string passName) {
        return Serilog.Context.LogContext.PushProperty("PassContext", $" [{passName}]");
    }
}