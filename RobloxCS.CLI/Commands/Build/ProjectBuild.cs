using System.Diagnostics;
using RobloxCS.CLI.Reporting;
using RobloxCS.CompilerPipeline;
using RobloxCS.CompilerPipeline.Documents;
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
        CancellationToken cancellation
    ) {
        var stopwatch = Stopwatch.StartNew();
        reporter.Begin(location.Name);

        var openDiagnostics = new List<Diagnostic>();
        var opened = await reporter.RunAsync("load", $"Loading [white]{Markup.Escape(location.Name)}[/]", _ =>
            ProjectSession.OpenAsync(location.SolutionFile, location.ProjectFile, options, openDiagnostics, cancellation));
        reporter.PrintDiagnostics(openDiagnostics);

        if (!opened.Ok) {
            reporter.PrintDiagnostic(opened.Diagnostic);
            reporter.Summary(0, 0, stopwatch.Elapsed);

            return new BuildOutcome(null, 0, 0);
        }

        var session = opened.Value;
        if (openDiagnostics.Any(diagnostic => diagnostic.IsError)) {
            session.Dispose();
            reporter.Summary(0, 0, stopwatch.Elapsed);

            return new BuildOutcome(null, 0, 0);
        }

        try {
            var total = session.Plans.Sum(plan => plan.Project.Documents.Count(Filters.ShouldCompile));
            var outcome = await reporter.RunAsync("compile", $"Compiling {total} {(total == 1 ? "file" : "files")}", async progress => {
                var compiled = 0;
                var failed = 0;

                await foreach (var result in session.CompileAllAsync(cancellation)) {
                    if (result.Output.Ok) {
                        reporter.PrintDiagnostics(result.Diagnostics);
                        compiled++;
                    } else {
                        reporter.Failure(result);
                        failed++;
                    }

                    progress($"Compiling [white]{Markup.Escape(result.Document.Name)}[/] [grey]· {compiled + failed}/{total}[/]");
                }

                return new BuildOutcome(session, compiled, failed);
            });

            reporter.Summary(outcome.Compiled, outcome.Failed, stopwatch.Elapsed);

            return outcome;
        } catch {
            session.Dispose();

            throw;
        }
    }
}