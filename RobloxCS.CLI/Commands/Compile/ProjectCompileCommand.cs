using System.ComponentModel;
using System.Diagnostics;
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

        var outDir = Path.Combine(fullCwd, "out");

        var candidateMatcher = new Matcher().AddInclude("*.csproj");
        var csprojCandidates = candidateMatcher.GetResultsInFullPath(fullCwd).ToList();

        if (csprojCandidates.Count == 0) {
            Log.Error("Failed to find a .csproj file in the current directory.");

            return -1;
        }

        var csprojFile = csprojCandidates.First();
        Log.Information("Starting project handling for {ProjectFilePath}", csprojFile);

        var watch = Stopwatch.StartNew();

        MSBuildLocator.RegisterDefaults();
        Log.Information("Registered MSBuild defaults");

        using var workspace = MSBuildWorkspace.Create();
        Log.Information("Created MSBuild workspace");

        var project = await workspace.OpenProjectAsync(csprojFile, cancellationToken: cancellation);
        Log.Information("Read MSBuild properties");

        var compilation = await project.GetCompilationAsync(cancellation);
        if (compilation is null) {
            Log.Error("Failed to get compilation from MSBuild.");

            return -1;
        }

        Log.Information("Got C# compilation");

        // TODO: Support this
        // string intermediatesFolderName;
        // if (project.CompilationOutputInfo.GeneratedFilesOutputDirectory is null) {
        //     Log.Verbose("Generated files output directory is null, presuming `obj`");
        //     intermediatesFolderName = "obj";
        // } else {
        //     intermediatesFolderName = project.CompilationOutputInfo.GeneratedFilesOutputDirectory;
        // }

        watch.Stop();
        Log.Information("Finished project handling for {ProjectFilePath} in {ElapsedMillis}ms", csprojFile, watch.ElapsedMilliseconds);

        foreach (var document in project.Documents) {
            if (document.Folders is ["obj", ..]) {
                Log.Verbose("Skipping {DocumentName} as it is inside intermediates folder", document.Name);

                continue;
            }

            var syntaxTree = await document.GetSyntaxTreeAsync(cancellation);
            if (syntaxTree is null) {
                Log.Error("Failed to get syntax tree for document {DocumentName}", document.Name);

                return 1;
            }

            var compiler = new CSharpCompiler(syntaxTree, compilation);

            var diags = compiler.FormatDiagnostics();
            foreach (var diag in diags) {
                _console.MarkupLine(diag);
            }

            var transpiler = new CSharpTranspiler(new TranspilerOptions(ScriptType.Module), compiler);
            var chunk = transpiler.Transpile();

            var renderer = new RendererWalker();
            var code = renderer.Render(chunk);

            var filename = Path.GetFileNameWithoutExtension(syntaxTree.FilePath);
            var outPath = Path.Combine(outDir, $"{filename}.luau");

            Log.Verbose("Writing output to {OutFilePath}", outPath);
            Directory.CreateDirectory(outDir);

            await File.WriteAllTextAsync(outPath, code, cancellation);
        }

        return 0;
    }
}