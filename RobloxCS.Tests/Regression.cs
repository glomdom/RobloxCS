using System.Diagnostics;
using RobloxCS.Compiler;
using RobloxCS.Renderer;
using RobloxCS.Transpiler;

namespace RobloxCS.Tests;

[TestFixture]
public class Regression {
    [OneTimeSetUp]
    public void OneTimeSetup() {
        EnsureLuneInstalled();
    }

    [Test, TestCaseSource(nameof(GetFromFiles))]
    public void FileTest(string inputPath, string expected) {
        var luneProc = Process.Start(new ProcessStartInfo {
            FileName = "lune",
            Arguments = $"run {inputPath}",
            RedirectStandardOutput = true,
        });

        if (luneProc is null) throw new Exception("Lune is not installed or available in PATH.");

        luneProc.WaitForExit();

        var output = luneProc.StandardOutput.ReadToEnd();
        
        Assert.That(output, Is.EqualTo(expected));
    }

    private static void EnsureLuneInstalled() {
        var luneProc = Process.Start(new ProcessStartInfo {
            FileName = "lune",
            Arguments = "--version",
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            RedirectStandardInput = false,
        });

        if (luneProc is null) throw new Exception("Lune is not installed or available in PATH.");

        luneProc.WaitForExit();
    }

    private static IEnumerable<TestCaseData> GetFromFiles() {
        GenerateLuauFiles();
        
        var workDir = TestContext.CurrentContext.WorkDirectory;
        var dataDir = Path.Join(workDir, "Data/");
        var outDir = Path.Join(dataDir, "Output/");
        var expectedDir = Path.Join(dataDir, "Expected/");

        if (!Directory.Exists(dataDir)) {
            throw new Exception("Data folder not found. Cannot proceed to generate tests.");
        }
        
        foreach (var path in Directory.GetFiles(outDir)) {
            var fileName = Path.GetFileNameWithoutExtension(path);
            var expectedPath = Path.Join(expectedDir, fileName + ".out");
            var expected = File.ReadAllText(expectedPath);

            yield return new TestCaseData(path, expected).SetName($"{fileName}");
        }
    }

    private static void GenerateLuauFiles() {
        var workDir = TestContext.CurrentContext.WorkDirectory;
        var dataDir = Path.Join(workDir, "Data/");
        var outDir = Path.Join(dataDir, "Output/");

        if (!Directory.Exists(dataDir)) {
            throw new Exception("Data folder not found. Cannot proceed to generate tests.");
        }

        Directory.CreateDirectory(outDir);

        foreach (var path in Directory.GetFiles(dataDir)) {
            var output = TranspileFile(path);
            var name = Path.GetFileNameWithoutExtension(path) + ".luau";

            File.WriteAllText(Path.Join(outDir, name), output);
        }
    }

    private static string TranspileFile(string path) {
        var transpiler = new CSharpTranspiler(new TranspilerOptions(ScriptType.Module), new CSharpCompiler(path));

        var chunk = transpiler.Transpile();
        var renderer = new RendererWalker();
        var output = renderer.Render(chunk);

        return output;
    }
}