using System.Diagnostics;
using RobloxCS.Common;
using Spectre.Console;

namespace RobloxCS.CLI.Reporting;

internal static class DotnetRestore {
    // after packages change, so IDEs see the new project references without a manual reload
    public static Task<bool> RunForProjectAsync(IAnsiConsole console, string root, CancellationToken cancellation) {
        var solution = RobloxCS.Packages.SolutionPackages.FindSolutions(root).FirstOrDefault();

        return solution is null ? Task.FromResult(true) : RunAsync(console, solution, cancellation);
    }

    public static Task<bool> RunAsync(IAnsiConsole console, string solution, CancellationToken cancellation) {
        var terminal = new TerminalOutput(console);

        return terminal.RunAsync("restore", "Restoring .NET dependencies", async _ => {
            var start = new ProcessStartInfo("dotnet", ["restore", solution]) {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };

            foreach (var key in start.Environment.Keys.Where(key => key.StartsWith("MSBuild", StringComparison.OrdinalIgnoreCase)).ToList()) {
                start.Environment.Remove(key);
            }

            using var process = Process.Start(start);
            if (process is null) {
                terminal.Write("error", "red", "Could not start dotnet restore.");

                return false;
            }

            var stdout = process.StandardOutput.ReadToEndAsync(cancellation);
            var stderr = process.StandardError.ReadToEndAsync(cancellation);

            try {
                await process.WaitForExitAsync(cancellation);
            } catch (OperationCanceledException) {
                if (!process.HasExited) {
                    process.Kill(entireProcessTree: true);
                }

                throw;
            }

            var output = await stdout;
            var errors = await stderr;
            if (process.ExitCode == 0) {
                return true;
            }

            terminal.Write("error", "red", "dotnet restore failed.");
            if (!string.IsNullOrWhiteSpace(output)) {
                terminal.Write("restore", "red", Markup.Escape(output.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(errors)) {
                terminal.Write("restore", "red", Markup.Escape(errors.Trim()));
            }

            return false;
        });
    }
}