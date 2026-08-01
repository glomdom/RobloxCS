using Microsoft.Extensions.FileSystemGlobbing;
using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using Serilog;

namespace RobloxCS.CompilerPipeline.Solutions;

public static class SolutionDiscovery {
    public static Result<string> Discover(string path) {
        var fullCwd = Path.GetFullPath(path);

        var candidateMatcher = new Matcher().AddInclude("*.slnx");
        var slnCandidates = candidateMatcher.GetResultsInFullPath(fullCwd).ToList();

        if (slnCandidates.Count == 0) {
            Log.Error("Failed to find a .slnx file in {SearchDirectory}.", fullCwd);

            return Diagnostic.Error(DiagnosticId.NoSolutionFound, $"Failed to find a .slnx file in '{fullCwd}'.");
        }

        return slnCandidates.First();
    }
}