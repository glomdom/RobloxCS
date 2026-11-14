using System.Globalization;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Spectre.Console;

namespace RobloxCS.Common;

public sealed class TerminalOutput : ILogEventSink {
    private static readonly MessageTemplateTextFormatter MessageFormatter = new("{Message:lj}", CultureInfo.InvariantCulture);
    private readonly IAnsiConsole _console;

    public TerminalOutput(IAnsiConsole console) {
        _console = console;
    }

    public void Write(string label, string color, string markup) {
        var grid = new Grid();
        grid.AddColumn(new GridColumn { NoWrap = true, Width = 18, Padding = new Padding(0, 0, 2, 0) });
        grid.AddColumn(new GridColumn { Padding = new Padding(0) });
        grid.AddRow(
            new Markup($"[grey]{DateTime.Now:HH:mm:ss}[/]  [{color}]{Markup.Escape(label.PadRight(8))}[/]"),
            new Markup(markup)
        );
        _console.Write(grid);
    }

    public async Task<T> RunAsync<T>(string label, string markup, Func<Action<string>, Task<T>> action, bool announce = true) {
        if (announce) {
            Write(label, "cyan", markup);
        }

        if (!_console.Profile.Capabilities.Interactive || !_console.Profile.Capabilities.Ansi || LoggerSetup.IsVerbose) {
            return await action(_ => {});
        }

        return await _console.Status().Spinner(Spinner.Known.Dots).SpinnerStyle(new Style(Color.Cyan))
            .StartAsync(markup, context => action(message => context.Status = message));
    }

    public void Emit(LogEvent logEvent) {
        var (label, color) = logEvent.Level switch {
            LogEventLevel.Verbose => ("trace", "grey"),
            LogEventLevel.Debug => ("debug", "grey"),
            LogEventLevel.Information => ("info", "blue"),
            LogEventLevel.Warning => ("warning", "yellow"),
            _ => ("error", "red"),
        };
        var pass = logEvent.Properties.TryGetValue("PassContext", out var context) && context is ScalarValue { Value: string name }
            ? $"[grey]{Markup.Escape(name)} ·[/] "
            : string.Empty;

        using var message = new StringWriter(CultureInfo.InvariantCulture);
        MessageFormatter.Format(logEvent, message);
        var text = message.ToString();
        if (logEvent.Properties.TryGetValue("ProjectRoot", out var root) && root is ScalarValue { Value: string directory }) {
            var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            text = text.Replace(Path.TrimEndingDirectorySeparator(directory) + Path.DirectorySeparatorChar, string.Empty, comparison);
        }

        Write(label, color, pass + Markup.Escape(text));

        if (logEvent.Exception is { } exception) {
            if (LoggerSetup.IsVerbose) {
                Write("detail", "grey", Markup.Escape(exception.ToString()));
            } else {
                Write("detail", "grey", $"{Markup.Escape(exception.Message)} [grey](use --verbose for the stack trace)[/]");
            }
        }
    }
}