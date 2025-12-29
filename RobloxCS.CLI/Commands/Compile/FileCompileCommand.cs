// ReSharper disable ClassNeverInstantiated.Global

using System.ComponentModel;
using RobloxCS.Common;
using RobloxCS.Compiler;
using RobloxCS.Renderer;
using RobloxCS.Transpiler;
using Serilog;
using Serilog.Events;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Compile;

[Description("Compile a C# file's source into Luau code.")]
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
        Logger.LevelSwitch.MinimumLevel = settings.Verbosity ? LogEventLevel.Verbose : LogEventLevel.Warning;

        Log.Information("Creating C# compiler");
        var compiler = new CSharpCompiler(settings.Path);
        var diagnosticMessages = compiler.FormatDiagnostics();

        TranspilerOptions options;
        if (settings.Path.EndsWith("client.cs")) {
            options = new TranspilerOptions(ScriptType: ScriptType.Local);
        } else if (settings.Path.EndsWith("server.cs")) {
            options = new TranspilerOptions(ScriptType: ScriptType.Server);
        } else {
            options = new TranspilerOptions(ScriptType: ScriptType.Module);
        }

        foreach (var diagnostic in diagnosticMessages) {
            _console.MarkupLine(diagnostic);
        }

        Log.Information("Creating C# transpiler");
        var transpiler = new CSharpTranspiler(options, compiler);
        var chunk = transpiler.Transpile();

        Log.Information("Starting to render nodes");

        var t = new RendererWalker();
        var output = t.Render(chunk);

        Console.WriteLine(output);

        return 0;
    }
}