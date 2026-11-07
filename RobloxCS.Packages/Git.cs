using System.Diagnostics;

namespace RobloxCS.Packages;

internal static class Git {
    public static async Task<string> RunAsync(string? workingDirectory, CancellationToken cancellation, params string[] arguments) {
        var start = new ProcessStartInfo("git") {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        if (workingDirectory is not null) {
            start.WorkingDirectory = workingDirectory;
        }

        foreach (var argument in arguments) {
            start.ArgumentList.Add(argument);
        }

        start.Environment["GIT_TERMINAL_PROMPT"] = "0";

        Process? process;

        try {
            process = Process.Start(start);
        } catch (System.ComponentModel.Win32Exception) {
            throw new PackageException("git was not found, install it and make sure it's on PATH");
        }

        if (process is null) {
            throw new PackageException("failed to start git");
        }

        using (process) {
            var stdout = process.StandardOutput.ReadToEndAsync(cancellation);
            var stderr = process.StandardError.ReadToEndAsync(cancellation);

            await process.WaitForExitAsync(cancellation);

            if (process.ExitCode != 0) {
                throw new PackageException($"git {string.Join(' ', arguments)} failed:\n{(await stderr).Trim()}");
            }

            return await stdout;
        }
    }

    public static async Task<Dictionary<string, string>> ListTagsAsync(string url, CancellationToken cancellation) {
        var output = await RunAsync(null, cancellation, "ls-remote", "--tags", url);
        var tags = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var line in output.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)) {
            var parts = line.Split('\t');
            if (parts.Length != 2 || !parts[1].StartsWith("refs/tags/", StringComparison.Ordinal)) {
                continue;
            }

            var tag = parts[1]["refs/tags/".Length..];

            if (tag.EndsWith("^{}", StringComparison.Ordinal)) {
                tags[tag[..^3]] = parts[0];
            } else {
                tags.TryAdd(tag, parts[0]);
            }
        }

        return tags;
    }
}