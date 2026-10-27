using System.Diagnostics;
using RobloxCS.CLI.Reporting;
using RobloxCS.CompilerPipeline;
using Spectre.Console;
using Diagnostic = RobloxCS.Common.Diagnostics.Diagnostic;

namespace RobloxCS.CLI.Commands.Build;

internal sealed record BuildOutcome(ProjectSession? Session, int Compiled, int Failed) {
    public bool Ok => Session is not null && Failed == 0;
}

internal static class ProjectBuild {
    public static async Task<BuildOutcome> OpenAndCompileAsync(
        BuildReporter reporter,
        ProjectLocation location,
        EmitOptions options,
        string? summarySuffix,
        CancellationToken cancellation
    ) {
        var stopwatch = Stopwatch.StartNew();

        reporter.Status($"building [white]{Markup.Escape(location.Name)}[/]");

        var openDiagnostics = new List<Diagnostic>();
        var opened = await ProjectSession.OpenAsync(location.SolutionFile, location.ProjectFile, options, openDiagnostics, cancellation);
        
        reporter.PrintDiagnostics(openDiagnostics);

        if (!opened.Ok) {
            reporter.PrintDiagnostic(opened.Diagnostic);

            return new BuildOutcome(null, 0, 0);
        }

        var session = opened.Value;

        reporter.Status($"created session [white]{Markup.Escape(location.Name)}[/]");

        if (openDiagnostics.Any(diagnostic => diagnostic.IsError)) {
            session.Dispose();

            return new BuildOutcome(null, 0, 0);
        }

        var compiled = 0;
        var failed = 0;

        reporter.Status($"compiling [white]{Markup.Escape(location.Name)}[/]");
        await foreach (var result in session.CompileAllAsync(cancellation)) {
            if (result.Output.Ok) {
                reporter.PrintDiagnostics(result.Diagnostics);
                compiled++;

                continue;
            }

            reporter.Failure(result);
            failed++;
        }

        reporter.Summary(compiled, failed, stopwatch.Elapsed, summarySuffix);

        return new BuildOutcome(session, compiled, failed);
    }
}