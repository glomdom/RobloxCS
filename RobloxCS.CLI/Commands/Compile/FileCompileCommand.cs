using System.ComponentModel;
using JetBrains.Annotations;
using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using RobloxCS.CompilerPipeline.Documents;
using Serilog.Events;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Compile;

[Description("Compile a C# file's source into Luau code.")]
[UsedImplicitly]
public sealed class FileCompileCommand : Command<FileCompileCommand.Settings> {
    private readonly IAnsiConsole _console;

    public FileCompileCommand(IAnsiConsole console) {
        _console = console;
    }

    public sealed class Settings : CompileSettings {
        [CommandArgument(0, "<path>")]
        [Description("The path of the [green]C#[/] file to compile.")]
        public required string Path { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellation) {
        LoggerSetup.LevelSwitch.MinimumLevel = settings.Verbosity ? LogEventLevel.Verbose : LogEventLevel.Warning;

        List<Diagnostic> diagnostics = [];
        var result = FileCompiler.Compile(settings.Path, settings.TypesFilePath, settings.SkipDiagnostics, diagnostics);

        foreach (var d in diagnostics) {
            _console.WriteLine(d.Render());
        }

        if (!result.Ok) return Fail(result.Diagnostic);

        Console.WriteLine(result.Value);

        return 0;
    }

    private int Fail(Diagnostic diag) {
        _console.MarkupLine(diag.Render());

        return -1;
    }
}