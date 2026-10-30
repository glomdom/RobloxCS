using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using RobloxCS.Compiler;
using RobloxCS.Lowering;
using RobloxCS.Renderer;
using RobloxCS.Transpiler;

namespace RobloxCS.Tests;

[TestFixture]
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

    private static string Transpile(string path) =>
        TranspileCache.GetOrAdd(path, static p => {
            var transpiler = new CSharpTranspiler(
                new TranspilerOptions(ScriptType.Local),
                new CSharpCompiler(p, "RobloxCS.Types.dll", true)
            );

            var result = transpiler.Transpile();
            if (!result.Ok) {
                Assert.Fail($"Failed to transpile '{p}':\n" + string.Join("\n", result.Diagnostics.Select(d => d.Render())));
            }

            return new RendererWalker().Render(result.Chunk!);
        });

    private static string RunWithLuau(string path) {
        var startInfo = new ProcessStartInfo {
            FileName = "luau",
            Arguments = path,
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
            Arguments = path,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        Process? process;
        try {
            process = Process.Start(startInfo);
        } catch (Exception ex) {
            throw new Exception("luau-analuze is not installed or not on PATH.", ex);
        }

        if (process is null) throw new Exception("Failed to start luau.");

        // waiting first deadlocks >4kb
        var stderrTask = process.StandardError.ReadToEndAsync();

        if (!process.WaitForExit(30_000)) {
            process.Kill(entireProcessTree: true);

            throw new Exception($"luau-analyze timed out after 30s running '{path}'.");
        }

        var stderr = stderrTask.GetAwaiter().GetResult();

        if (process.ExitCode != 0) {
            throw new Exception($"luau-analyze failed running '{path}'.\n{stderr}");
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

            yield return new TestCaseData(name).SetName(name);
        }
    }

    private static string Normalize(string text) => text.ReplaceLineEndings("\n").TrimEnd('\n');
    private static string InputPath(string name) => Path.Join(DataDir, name + ".cs");

    private static string BaseDir => TestContext.CurrentContext.TestDirectory;
    private static string DataDir => Path.Join(BaseDir, "Data");
    private static string ExpectedDir => Path.Join(DataDir, "Expected");
    private static string OutputDir => Path.Join(DataDir, "Output");
}