using System.ComponentModel;
using System.Diagnostics;
using JetBrains.Annotations;
using RobloxCS.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Init;

[Description("Create a new RobloxCS project.")]
[UsedImplicitly]
public sealed class InitCommand : Command<InitSettings> {
    private readonly IAnsiConsole _console;

    public InitCommand(IAnsiConsole console) {
        _console = console;
    }

    protected override int Execute(CommandContext context, InitSettings settings, CancellationToken cancellation) {
        LoggerSetup.ApplyVerbosity(settings.Verbosity);

        var target = settings.Name is null ? Environment.CurrentDirectory : Path.GetFullPath(settings.Name);
        var rawName = settings.Name ?? Path.GetFileName(Path.TrimEndingDirectorySeparator(target));

        if (ProjectTemplate.SanitizeName(rawName) is not { } name) {
            _console.MarkupLine($"[red]error:[/] '{Markup.Escape(rawName)}' can't be turned into a project name. Pass one explicitly: [white]rbxcsc init MyGame[/]");

            return -1;
        }

        var files = ProjectTemplate.Files(name, TypesInstall.Version);

        var conflicts = files.Keys
            .Where(path => !ProjectTemplate.WriteIfMissing.Contains(path))
            .Where(path => File.Exists(Path.Combine(target, path)))
            .ToList();

        if (conflicts.Count > 0 && !settings.Force) {
            _console.MarkupLine($"[red]error:[/] {Markup.Escape(target)} already contains a project:");

            foreach (var conflict in conflicts) {
                _console.MarkupLine($"  [grey]{Markup.Escape(conflict)}[/]");
            }

            _console.MarkupLine("[cyan]note:[/] pass [white]--force[/] to overwrite them");

            return -1;
        }

        TypesInstall.EnsureInstalled();

        foreach (var (relative, content) in files) {
            var path = Path.Combine(target, relative);

            if (ProjectTemplate.WriteIfMissing.Contains(relative) && File.Exists(path)) {
                continue;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, content);

            _console.MarkupLine($"[green]created[/] [grey]{Markup.Escape(relative)}[/]");
        }
        
        var restore = Process.Start(new ProcessStartInfo("dotnet", ["restore", Path.Combine(target, $"{name}.slnx")]) {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        });

        restore?.WaitForExit();
        
        _console.MarkupLine($"[green]restored[/] [grey]{Markup.Escape(target)}[/]");

        _console.WriteLine();
        _console.MarkupLine($"[green]{Markup.Escape(name)}[/] is ready. Next:");

        if (settings.Name is not null) {
            _console.MarkupLine($"  [white]cd {Markup.Escape(settings.Name)}[/]");
        }

        _console.MarkupLine("  [white]rbxcsc build --watch[/]");
        _console.MarkupLine("  [white]rojo serve[/]");

        return 0;
    }
}