using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using RobloxCS.CompilerPipeline.Solutions;

namespace RobloxCS.CLI.Commands.Build;

internal sealed record ProjectLocation(string Root, string SolutionFile, string ProjectFile) {
    public string Name => Path.GetFileNameWithoutExtension(SolutionFile);
}

internal static class ProjectLocator {
    public static Result<ProjectLocation> Locate(string directory, string projectJsonPath) {
        var discovery = SolutionDiscovery.Discover(directory);
        if (!discovery.Ok) {
            return discovery.Diagnostic;
        }

        var solutionFile = Path.GetFullPath(discovery.Value);
        var projectFile = Path.GetFullPath(Path.Combine(directory, projectJsonPath));

        if (!File.Exists(projectFile)) {
            return Diagnostic.Error(DiagnosticId.FileNotFound, $"Failed to find Rojo project '{projectJsonPath}' in '{directory}'.");
        }

        return new ProjectLocation(Path.GetDirectoryName(solutionFile)!, solutionFile, projectFile);
    }
}