using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using RobloxCS.Compiler;
using RobloxCS.Renderer;
using RobloxCS.Transpiler;

namespace RobloxCS.Tests;

[TestFixture]
public class Regression {
    private static readonly ConcurrentDictionary<string, string> TranspileCache = new();

    [Test, TestCaseSource(nameof(GetCases))]
    public void Golden(string name) {
        var actual = Normalize(Transpile(InputPath(name)));
        var goldenPath = Path.Join(_goldenDir, name + ".luau");

        if (!File.Exists(goldenPath)) {
            Assert.Fail(
                $"No golden file for '{name}'. Run the UpdateGoldens test to create it, " + "then review the diff before committing."
            );
        }

        Assert.That(actual, Is.EqualTo(Normalize(File.ReadAllText(goldenPath))));
    }


    [Test, TestCaseSource(nameof(GetCases))]
    public void Behaviour(string name) {
        var outputPath = Path.Join(_outputDir, name + ".luau");

        Directory.CreateDirectory(_outputDir);
        File.WriteAllText(outputPath, Transpile(InputPath(name)));

        var expectedPath = Path.Join(_expectedDir, name + ".out");
        if (!File.Exists(expectedPath)) {
            Assert.Fail($"No expected output for '{name}'. Create {expectedPath}.");
        }

        var actual = RunWithLune(outputPath);

        Assert.That(Normalize(actual), Is.EqualTo(Normalize(File.ReadAllText(expectedPath))));
    }

    [Test, Explicit("Overwrites committed golden files."), Category("UpdateGolden")]
    public void UpdateGoldens() {
        Directory.CreateDirectory(_sourceGoldenDir);

        foreach (var testCase in GetCases()) {
            var name = (string)testCase.Arguments[0]!;
            var rendered = Normalize(Transpile(InputPath(name)));

            File.WriteAllText(Path.Join(_sourceGoldenDir, name + ".luau"), rendered + "\n");

            TestContext.Out.WriteLine($"wrote {name}.luau");
        }

        TestContext.Out.WriteLine($"goldens written to {_sourceGoldenDir}");
    }


    private static string Transpile(string path) =>
        TranspileCache.GetOrAdd(path, static p => {
            var transpiler = new CSharpTranspiler(
                new TranspilerOptions(ScriptType.Local),
                new CSharpCompiler(p, "RobloxCS.Types.dll", true)
            );

            var chunk = transpiler.Transpile();
            var renderer = new RendererWalker();

            return renderer.Render(chunk);
        });

    private static string RunWithLune(string path) {
        var startInfo = new ProcessStartInfo {
            FileName = "lune",
            Arguments = $"run {path}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        Process? process;
        try {
            process = Process.Start(startInfo);
        } catch (Exception ex) {
            throw new Exception("Lune is not installed or not on PATH.", ex);
        }

        if (process is null) throw new Exception("Failed to start lune.");

        // waiting first deadlocks >4kb
        var stdoutTask = process.StandardOutput.ReadToEndAsync();
        var stderrTask = process.StandardError.ReadToEndAsync();

        if (!process.WaitForExit(30_000)) {
            process.Kill(entireProcessTree: true);

            throw new Exception($"Lune timed out after 30s running '{path}'.");
        }

        var stdout = stdoutTask.GetAwaiter().GetResult();
        var stderr = stderrTask.GetAwaiter().GetResult();

        if (process.ExitCode != 0) {
            throw new Exception($"Lune exited with {process.ExitCode} running '{path}'.\n{stderr}");
        }

        return stdout;
    }

    private static IEnumerable<TestCaseData> GetCases() {
        if (!Directory.Exists(_dataDir)) {
            throw new DirectoryNotFoundException($"Test data directory not found: {_dataDir}");
        }

        var inputs = Directory.GetFiles(_dataDir, "*.cs", SearchOption.TopDirectoryOnly);
        if (inputs.Length == 0) {
            throw new InvalidOperationException($"No .cs test inputs found in {_dataDir}");
        }

        foreach (var path in inputs.OrderBy(static p => p, StringComparer.Ordinal)) {
            var name = Path.GetFileNameWithoutExtension(path);

            yield return new TestCaseData(name).SetName(name);
        }
    }

    private static string Normalize(string text) => text.ReplaceLineEndings("\n").TrimEnd('\n');
    private static string InputPath(string name) => Path.Join(_dataDir, name + ".cs");

    private static string _baseDir => TestContext.CurrentContext.TestDirectory;
    private static string _dataDir => Path.Join(_baseDir, "Data");
    private static string _goldenDir => Path.Join(_dataDir, "Golden");
    private static string _expectedDir => Path.Join(_dataDir, "Expected");
    private static string _outputDir => Path.Join(_dataDir, "Output");

    private static string _sourceGoldenDir {
        get {
            var projectDir = typeof(Regression).Assembly
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .FirstOrDefault(static a => a.Key == "ProjectDir")?.Value;

            if (string.IsNullOrEmpty(projectDir)) {
                throw new InvalidOperationException(
                    "ProjectDir assembly metadata is missing."
                );
            }

            return Path.Join(projectDir, "Data", "Golden");
        }
    }
}