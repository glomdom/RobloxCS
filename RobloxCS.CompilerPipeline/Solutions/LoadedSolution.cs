using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace RobloxCS.CompilerPipeline.Solutions;

/// <summary>
/// A <see cref="Solution"/> is a snapshot. It MUST have an alive reference
/// to the <see cref="MSBuildWorkspace"/> that it was created in.
/// </summary>
public sealed class LoadedSolution : IDisposable {
    private readonly MSBuildWorkspace _workspace;

    public Solution Solution { get; }
    public string Name { get; }

    internal LoadedSolution(MSBuildWorkspace workspace, Solution solution, string name) {
        _workspace = workspace;
        Solution = solution;
        Name = name;
    }

    public void Dispose() {
        _workspace.Dispose();
    }
}