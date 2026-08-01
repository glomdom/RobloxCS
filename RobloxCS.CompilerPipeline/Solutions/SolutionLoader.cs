using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using Diagnostic = RobloxCS.Common.Diagnostics.Diagnostic;

namespace RobloxCS.CompilerPipeline.Solutions;

public static class SolutionLoader {
    public static async Task<Result<LoadedSolution>> LoadAsync(string slnFile, CancellationToken cancellation) {
        var workspace = MSBuildWorkspace.Create();

        try {
            workspace.LoadMetadataForReferencedProjects = true;

            var failures = new List<string>();
            workspace.RegisterWorkspaceFailedHandler(e => {
                if (e.Diagnostic.Kind == WorkspaceDiagnosticKind.Failure) {
                    failures.Add(e.Diagnostic.Message);
                }
            });

            var solution = await workspace.OpenSolutionAsync(slnFile, cancellationToken: cancellation);
            var name = Path.GetFileNameWithoutExtension(slnFile);

            if (failures.Count > 0) {
                workspace.Dispose();

                return Diagnostic.Error(
                    DiagnosticId.SolutionLoadFailed,
                    $"Solution '{name}' did not load cleanly:\n" +
                    string.Join('\n', failures.Select(x => $"  - {x}")) +
                    "\nIf any of these mentioned unresolved packages or a missing assets file, " +
                    "run 'dotnet restore' on the target solution."
                );
            }

            return new LoadedSolution(workspace, solution, name);
        } catch {
            workspace.Dispose();

            throw;
        }
    }
}