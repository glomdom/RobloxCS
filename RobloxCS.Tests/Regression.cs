using System.Diagnostics;
using System.Text;
using RobloxCS.Compiler;
using RobloxCS.Renderer;
using RobloxCS.Transpiler;

namespace RobloxCS.Tests;

[TestFixture]
public class Regression {
    [Test, TestCaseSource(nameof(GetFromFiles))]
    public void FileTest(string inputPath, string expected) {
        var output = TryWithLune(inputPath);

        Assert.That(output, Is.EqualTo(expected));
    }

    private static string TryWithLune(string path) {
        Process? luneProc;
        try {
            luneProc = Process.Start(new ProcessStartInfo {
                FileName = "lune",
                Arguments = $"run {path}",
                RedirectStandardOutput = true,
                RedirectStandardError = false,
                RedirectStandardInput = false,
            });

            if (luneProc is null) throw new Exception("Failed to start lune.");
        } catch (Exception ex) {
            throw new Exception("Lune is not installed or available in PATH.", ex);
        }

        luneProc.WaitForExit();

        return luneProc.StandardOutput.ReadToEnd();
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
            InjectInstantiate(Path.Join(outDir, name), Path.GetFileNameWithoutExtension(path));
        }
    }

    private static string TranspileFile(string path) {
        var transpiler = new CSharpTranspiler(new TranspilerOptions(ScriptType.Module), new CSharpCompiler(path));

        var chunk = transpiler.Transpile();
        var renderer = new RendererWalker();
        var output = renderer.Render(chunk);

        return output;
    }

    private static void InjectInstantiate(string path, string className) {
        var sb = new StringBuilder();
        sb.AppendLine($"local __inst = {className}.new()");
        sb.AppendLine("__inst:Main()");

        using var file = File.Open(path, FileMode.Append);
        file.Write(Encoding.UTF8.GetBytes(sb.ToString()));
    }
}