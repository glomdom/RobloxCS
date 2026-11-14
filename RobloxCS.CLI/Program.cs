using System.Reflection;
using System.Text;
using RobloxCS.CLI.Commands.Build;
using RobloxCS.CLI.Commands.Emit;
using RobloxCS.CLI.Commands.Init;
using RobloxCS.CLI.Commands.Package;
using RobloxCS.CLI.Commands.Package.Add;
using RobloxCS.CLI.Commands.Package.Remove;
using RobloxCS.CLI.Commands.Package.Restore;
using RobloxCS.Common;
using Serilog;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobloxCS.CLI;

internal static class Program {
    private static int Main(string[] args) {
        Console.OutputEncoding = new UTF8Encoding();

        var console = AnsiConsole.Create(new AnsiConsoleSettings { Out = new AnsiConsoleOutput(Console.Error) });
        AnsiConsole.Console = console;
        LoggerSetup.Configure(console);
        var output = new TerminalOutput(console);

        var app = new CommandApp();
        app.Configure(config => {
            config.Settings.Console = console;
            config.SetApplicationName("rbxcsc");
            config.SetApplicationVersion(typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "unk");
            config.UseStrictParsing();
            config.PropagateExceptions();

            config.AddCommand<InitCommand>("init");
            config.AddCommand<BuildCommand>("build");
            config.AddCommand<EmitCommand>("emit");

            config.AddBranch<PackageSettings>("package", package => {
                package.SetDescription("Manage project dependencies.");

                package.AddCommand<PackageAddCommand>("add").WithDescription("Add a dependency to the project.");
                package.AddCommand<PackageRemoveCommand>("remove").WithDescription("Remove a dependency from the project.");
                package.AddCommand<PackageRestoreCommand>("restore").WithDescription("Installs all added dependencies from the project.");
            }).WithAlias("pkg");
        });

        try {
            return app.Run(args);
        } catch (CommandParseException ex) {
            if (ex.Pretty is { } pretty) {
                AnsiConsole.Write(pretty);
            } else {
                output.Write("error", "red", Markup.Escape(ex.Message));
            }

            return -1;
        } catch (CommandRuntimeException ex) {
            app.Run([..args, "--help"]);

            AnsiConsole.WriteLine();
            output.Write("error", "red", Markup.Escape(ex.Message));

            return -1;
        } catch (CommandAppException ex) {
            if (ex.Pretty is { } pretty) {
                AnsiConsole.Write(pretty);
            } else {
                output.Write("error", "red", Markup.Escape(ex.Message));
            }

            return -1;
        } catch (OperationCanceledException) {
            output.Write("stopped", "grey", "Operation cancelled.");

            return 130;
        } catch (Exception ex) {
            Log.Error(ex, "Unexpected compiler failure");
            output.Write("note", "cyan", "This may be a compiler bug. Please report it at https://github.com/glomdom/RobloxCS");

            return -1;
        } finally {
            Log.CloseAndFlush();
        }
    }
}