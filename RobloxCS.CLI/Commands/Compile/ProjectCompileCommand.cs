using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
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

    // TODO: Extract this into a config file perhaps
    private readonly Dictionary<string, string> _pathMapping = new() { { "Shared", "Shared" }, { "Server", "Server" }, { "Client", "Client" } };

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
        Log.Debug("Searching for candidate .sln files in {SearchDirectory}", fullCwd);

        var outDir = Path.Combine(fullCwd, "out");

        var candidateMatcher = new Matcher().AddInclude("*.slnx");
        var slnCandidates = candidateMatcher.GetResultsInFullPath(fullCwd).ToList();

        if (slnCandidates.Count == 0) {
            Log.Error("Failed to find a .slnx file in the current directory.");

            return -1;
        }

        var slnFile = slnCandidates.First();
        Log.Information("Starting project handling for {ProjectFilePath}", slnFile);

        var vsi = MSBuildLocator.RegisterDefaults();
        Log.Debug("Using dotnet {Version} in {DotnetPath}", vsi.Version, vsi.VisualStudioRootPath);
        Log.Debug("Found MSBuild executable in {ExecutablePath}", vsi.MSBuildPath);

        using var workspace = MSBuildWorkspace.Create();
        Log.Debug("Created MSBuild workspace");

        var solution = await workspace.OpenSolutionAsync(slnFile, cancellationToken: cancellation);
        var solutionName = Path.GetFileNameWithoutExtension(slnFile);
        Log.Verbose("Opened solution {SolutionName}", solutionName);

        foreach (var project in solution.Projects) {
            var compilation = await GetProjectCompilationAsync(project, cancellation);
            if (compilation is null) return -1;

            Log.Debug("Got C# compilation for {ProjectName}", project.Name);

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

                // FIXME: Slop code
                if (!_pathMapping.TryGetValue(project.Name.Split('.').Last(), out var dirName)) {
                    Log.Error("Failed to get directory mapping for {ProjectName}. Is it missing?", project.Name);

                    return -1;
                }

                var combinedOutDir = Path.Combine([outDir, dirName, ..document.Folders]);
                var outPath = Path.Combine(combinedOutDir, $"{filename}.luau");

                Directory.CreateDirectory(combinedOutDir);
                Log.Verbose("Writing output to {OutFilePath}", outPath);

                await File.WriteAllTextAsync(outPath, code, cancellation);
            }
        }

        // TODO: Support this
        // string intermediatesFolderName;
        // if (project.CompilationOutputInfo.GeneratedFilesOutputDirectory is null) {
        //     Log.Verbose("Generated files output directory is null, presuming `obj`");
        //     intermediatesFolderName = "obj";
        // } else {
        //     intermediatesFolderName = project.CompilationOutputInfo.GeneratedFilesOutputDirectory;
        // }

        return 0;
    }

    private static async Task<Compilation?> GetProjectCompilationAsync(Project project, CancellationToken cancellation) {
        var compilation = await project.GetCompilationAsync(cancellation);
        if (compilation is not null) return compilation;

        Log.Error("Failed to get compilation from MSBuild.");

        return null;
    }
}