using System.ComponentModel;
using JetBrains.Annotations;
using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using RobloxCS.CompilerPipeline;
using RobloxCS.CompilerPipeline.Documents;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Emit;

[Description("Transpile a single C# file and print the Luau to stdout.")]
[UsedImplicitly]
public sealed class EmitCommand : Command<EmitCommand.Settings> {
    private readonly IAnsiConsole _console;

    public EmitCommand(IAnsiConsole console) {
        _console = console;
    }

    public sealed class Settings : CommonSettings {
        [CommandArgument(0, "<path>")]
        [Description("The [green]C#[/] file to transpile")]
        public required string Path { get; init; }

        [CommandOption("--types-file")]
        [Description("Path to the assembly containing the Roblox type definitions")]
        public string TypesFilePath { get; init; } = "./RobloxCS.Types.dll";

        [CommandOption("--no-diagnostics")]
        [Description("Don't run Roslyn diagnostics")]
        public bool SkipDiagnostics { get; init; }

        [CommandOption("--print-hir")]
        [Description("Dump the HIR before lowering")]
        public bool PrintHir { get; init; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellation) {
        LoggerSetup.ApplyVerbosity(settings.Verbosity);

        List<Diagnostic> diagnostics = [];
        var options = new EmitOptions(settings.PrintHir, settings.SkipDiagnostics);
        var result = FileCompiler.Compile(settings.Path, settings.TypesFilePath, settings.SkipDiagnostics, options, diagnostics);

        DiagnosticMarkup.PrintAll(_console, diagnostics);

        if (!result.Ok) {
            DiagnosticMarkup.Print(_console, result.Diagnostic);

            return -1;
        }

        Console.WriteLine(result.Value);

        return 0;
    }
}