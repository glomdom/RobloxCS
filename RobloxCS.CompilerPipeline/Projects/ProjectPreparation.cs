using Microsoft.CodeAnalysis;
using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using RobloxCS.Common.Rojo;
using Diagnostic = RobloxCS.Common.Diagnostics.Diagnostic;

namespace RobloxCS.CompilerPipeline.Projects;

public static class ProjectPreparation {
    public static async Task<Result<ProjectPlan>> PrepareProjectAsync(Project project, List<RojoAnchor> anchors, CancellationToken cancellation) {
        var compilation = await GetProjectCompilationAsync(project, cancellation);
        if (!compilation.Ok) return compilation.Diagnostic;

        if (!compilation.Value.References.Any()) {
            return Diagnostic.Error(
                DiagnosticId.NoMetadataReferences,
                $"Project {project.Name} compiled with zero metadata references. Try running `dotnet restore` on the target solution."
            );
        }

        if (!RojoProject.TryResolveAnchor(project.Name, anchors, out var anchor))
            return Diagnostic.Error(
                DiagnosticId.NoAnchorFound,
                $"No instance anchor found in rojo project for {project.Name}"
            );


        return new ProjectPlan(project, compilation.Value, anchor);
    }

    private static async Task<Result<Compilation>> GetProjectCompilationAsync(Project project, CancellationToken cancellation) {
        var compilation = await project.GetCompilationAsync(cancellation);
        if (compilation is not null) return compilation;

        return Diagnostic.Error(DiagnosticId.FailedToGetCompilation, $"Failed to get compilation from MSBuild for {project.Name}.");
    }
}