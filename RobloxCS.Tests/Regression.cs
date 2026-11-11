using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Loader;
using System.Reflection;
using RobloxCS.Compiler;
using RobloxCS.Lowering;
using RobloxCS.Renderer;
using RobloxCS.Transpiler;

namespace RobloxCS.Tests;

[TestFixture, NonParallelizable]
public class Regression {
    private static readonly ConcurrentDictionary<string, string> TranspileCache = new();

    [Test, TestCaseSource(nameof(GetCases))]
    public void Behaviour(string name) {
        var outputPath = Path.Join(OutputDir, name + ".luau");

        Directory.CreateDirectory(OutputDir);
        File.WriteAllText(outputPath, Transpile(InputPath(name)));

        var expectedPath = Path.Join(ExpectedDir, name + ".out");
        if (!File.Exists(expectedPath)) {
            Assert.Fail($"No expected output for '{name}'. Create {expectedPath}.");
        }

        var actual = RunWithLuau(outputPath);

        Assert.That(Normalize(actual), Is.EqualTo(Normalize(File.ReadAllText(expectedPath))));
    }

    [Test, TestCaseSource(nameof(GetCases))]
    public void Typecheck(string name) {
        var outputPath = Path.Join(OutputDir, name + ".tc.luau");

        Directory.CreateDirectory(OutputDir);
        File.WriteAllText(outputPath, Transpile(InputPath(name)));

        var actual = TypecheckWithLuau(outputPath);

        Assert.That(actual, Is.True);
    }

    [Test, TestCaseSource(nameof(GetCases))]
    public void MatchesCSharp(string name) {
        var outputPath = Path.Join(OutputDir, name + ".differential.luau");
        Directory.CreateDirectory(OutputDir);
        File.WriteAllText(outputPath, Transpile(InputPath(name)));

        var expected = RunWithCSharp(InputPath(name));
        File.WriteAllText(Path.Join(OutputDir, name + ".clr.out"), expected);

        Assert.That(Normalize(RunWithLuau(outputPath)), Is.EqualTo(Normalize(expected)));
    }

    private static string RunWithCSharp(string path) {
        var compiler = new CSharpCompiler(path, "RobloxCS.Types.dll", false);
        Assert.That(compiler.HasErrors, Is.False, string.Join("\n", compiler.Diagnostics));

        using var assemblyStream = new MemoryStream();
        var emitted = compiler.Compilation.Emit(assemblyStream);

        Assert.That(emitted.Success, Is.True, string.Join("\n", emitted.Diagnostics));
        assemblyStream.Position = 0;

        var context = new AssemblyLoadContext("Regression", isCollectible: true);
        context.Resolving += (_, name) => name.Name == "RobloxCS.Types"
            ? Assembly.LoadFrom(Path.GetFullPath("RobloxCS.Types.dll"))
            : null;

        try {
            var assembly = context.LoadFromStream(assemblyStream);
            var entry = assembly.GetTypes().SelectMany(type =>
                    type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                )
                .Single(method => method.CustomAttributes.Any(attribute => attribute.AttributeType.FullName == "RobloxCS.Types.Attributes.EntryPointAttribute"));

            var previousOutput = Console.Out;
            var previousCulture = CultureInfo.CurrentCulture;
            using var output = new StringWriter(CultureInfo.InvariantCulture);

            try {
                Console.SetOut(output);
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

                var instance = entry.IsStatic ? null : Activator.CreateInstance(entry.DeclaringType!, nonPublic: true);
                entry.Invoke(instance, null);
            } finally {
                Console.SetOut(previousOutput);
                CultureInfo.CurrentCulture = previousCulture;
            }

            return output.ToString();
        } finally {
            context.Unload();
        }
    }

    private static string Transpile(string path) =>
        TranspileCache.GetOrAdd(path, static p => {
            var transpiler = new CSharpTranspiler(
                new TranspilerOptions(ScriptType.Local),
                new CSharpCompiler(p, "RobloxCS.Types.dll", false)
            );

            Assert.That(transpiler.Compiler.HasErrors, Is.False, string.Join("\n", transpiler.Compiler.Diagnostics));

            var result = transpiler.Transpile();
            if (!result.Ok) {
                Assert.Fail($"Failed to transpile '{p}':\n" + string.Join("\n", result.Diagnostics.Select(d => d.Render())));
            }

            return new RendererWalker().Render(result.Chunk!);
        });

    private static string RunWithLuau(string path) {
        var startInfo = new ProcessStartInfo {
            FileName = "luau",
            ArgumentList = { path },
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        Process? process;
        try {
            process = Process.Start(startInfo);
        } catch (Exception ex) {
            throw new Exception("luau is not installed or not on PATH.", ex);
        }

        if (process is null) throw new Exception("Failed to start luau.");

        // waiting first deadlocks >4kb
        var stdoutTask = process.StandardOutput.ReadToEndAsync();
        var stderrTask = process.StandardError.ReadToEndAsync();

        if (!process.WaitForExit(30_000)) {
            process.Kill(entireProcessTree: true);

            throw new Exception($"luau timed out after 30s running '{path}'.");
        }

        var stdout = stdoutTask.GetAwaiter().GetResult();
        var stderr = stderrTask.GetAwaiter().GetResult();

        if (process.ExitCode != 0) {
            throw new Exception($"luau exited with {process.ExitCode} running '{path}'.\n{stderr}");
        }

        return stdout;
    }

    private static bool TypecheckWithLuau(string path) {
        var startInfo = new ProcessStartInfo {
            FileName = "luau-analyze",
            ArgumentList = { path },
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        Process? process;
        try {
            process = Process.Start(startInfo);
        } catch (Exception ex) {
            throw new Exception("luau-analyze is not installed or not on PATH.", ex);
        }

        if (process is null) throw new Exception("Failed to start luau.");

        // waiting first deadlocks >4kb
        var stdoutTask = process.StandardOutput.ReadToEndAsync();
        var stderrTask = process.StandardError.ReadToEndAsync();

        if (!process.WaitForExit(30_000)) {
            process.Kill(entireProcessTree: true);

            throw new Exception($"luau-analyze timed out after 30s running '{path}'.");
        }

        var stdout = stdoutTask.GetAwaiter().GetResult();
        var stderr = stderrTask.GetAwaiter().GetResult();

        if (process.ExitCode != 0) {
            throw new Exception($"luau-analyze failed running '{path}'.\n{stdout}\n{stderr}");
        }

        return true;
    }

    private static IEnumerable<TestCaseData> GetCases() {
        if (!Directory.Exists(DataDir)) {
            throw new DirectoryNotFoundException($"Test data directory not found: {DataDir}");
        }

        var inputs = Directory.GetFiles(DataDir, "*.cs", SearchOption.TopDirectoryOnly);
        if (inputs.Length == 0) {
            throw new InvalidOperationException($"No .cs test inputs found in {DataDir}");
        }

        foreach (var path in inputs.OrderBy(static p => p, StringComparer.Ordinal)) {
            var name = Path.GetFileNameWithoutExtension(path);

            yield return new TestCaseData(name).SetName($"{{m}}({name})");
        }
    }

    private static string Normalize(string text) => text.ReplaceLineEndings("\n").TrimEnd('\n');
    private static string InputPath(string name) => Path.Join(DataDir, name + ".cs");

    private static string BaseDir => TestContext.CurrentContext.TestDirectory;
    private static string DataDir => Path.Join(BaseDir, "Data");
    private static string ExpectedDir => Path.Join(DataDir, "Expected");
    private static string OutputDir => Path.Join(DataDir, "Output");
}