using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.FileSystemGlobbing;
using RobloxCS.Common;
using RobloxCS.Compiler;
using RobloxCS.Renderer;
using RobloxCS.Transpiler;
using Serilog;
using Serilog.Events;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Compile;

[Description("Compile a C# project.")]
[UsedImplicitly]
public sealed class ProjectCompileCommand : AsyncCommand<ProjectCompileCommand.Settings> {
    private readonly IAnsiConsole _console;

    public ProjectCompileCommand(IAnsiConsole console) {
        _console = console;
    }

    public sealed class Settings : CompileSettings {
        // [CommandArgument(0, "<path>")]
        // [Description("The path of the [green]C#[/] file to compile.")]
        // public required string Path { get; set; }
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellation) {
        LoggerSetup.LevelSwitch.MinimumLevel = settings.Verbosity ? LogEventLevel.Verbose : LogEventLevel.Warning;

        var fullCwd = Path.GetFullPath(Environment.CurrentDirectory);
        Log.Information("Searching for candidate .csproj files in {SearchDirectory}", fullCwd);

        var candidateMatcher = new Matcher().AddInclude("*.csproj");
        var csprojCandidates = candidateMatcher.GetResultsInFullPath(fullCwd).ToList();

        if (csprojCandidates.Count == 0) {
            Log.Error("Failed to find a .csproj file in the current directory.");

            return -1;
        }

        var csprojFile = csprojCandidates.First();
        Log.Information("Compiling project from {ProjectFilePath}", csprojFile);

        var vsi = MSBuildLocator.RegisterDefaults();
        Log.Information("Registered MSBuild defaults");

        using var workspace = MSBuildWorkspace.Create();
        Log.Information("Created MSBuild workspace");

        var project = await workspace.OpenProjectAsync(csprojFile, cancellationToken: cancellation);
        Log.Information("Read MSBuild properties");

        var compilation = await project.GetCompilationAsync(cancellation);
        Log.Information("Got C# compilation");

        // Log.Information("Creating C# compiler");
        // var compiler = new CSharpCompiler(settings.Path);
        // var diagnosticMessages = compiler.FormatDiagnostics();
        //
        // TranspilerOptions options;
        // if (settings.Path.EndsWith("client.cs")) {
        //     options = new TranspilerOptions(ScriptType: ScriptType.Local);
        // } else if (settings.Path.EndsWith("server.cs")) {
        //     options = new TranspilerOptions(ScriptType: ScriptType.Server);
        // } else {
        //     options = new TranspilerOptions(ScriptType: ScriptType.Module);
        // }
        //
        // foreach (var diagnostic in diagnosticMessages) {
        //     _console.MarkupLine(diagnostic);
        // }
        //
        // Log.Information("Creating C# transpiler");
        // var transpiler = new CSharpTranspiler(options, compiler);
        // var chunk = transpiler.Transpile();
        //
        // Log.Information("Starting to render nodes");
        //
        // var t = new RendererWalker();
        // var output = t.Render(chunk);
        //
        // Console.WriteLine(output);

        return 0;
    }
}