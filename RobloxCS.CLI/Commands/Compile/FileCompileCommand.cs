// ReSharper disable ClassNeverInstantiated.Global

using System.ComponentModel;
using RobloxCS.AST;
using RobloxCS.Transpiler;
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

    public override int Execute(CommandContext context, Settings settings) {
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

        var transpiler = new CSharpTranspiler(options, compiler);
        transpiler.Transpile();

        var renderer = new Renderer.Renderer(transpiler.Nodes);
        var output = renderer.Render();
        
        Console.WriteLine(output);

        return 0;
    }
}