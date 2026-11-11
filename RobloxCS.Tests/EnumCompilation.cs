using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RobloxCS.Compiler;
using RobloxCS.Lowering;
using RobloxCS.Modules;
using RobloxCS.Renderer;
using RobloxCS.Transpiler;

namespace RobloxCS.Tests;

[TestFixture]
public class EnumCompilation {
    private MetadataReference[] _references = [];

    [OneTimeSetUp]
    public void LoadReferences() {
        var path = Path.Join(TestContext.CurrentContext.TestDirectory, "Data", "Enums.client.cs");
        _references = [.. new CSharpCompiler(path, "RobloxCS.Types.dll", true).Compilation.References];
    }

    [Test]
    public void ImportsAndExportsEnums() {
        var declarations = Parse("namespace Shared; public enum State { Idle, Ready = 5 }", "State.cs");
        var consumer = Parse("""
            using RobloxCS.Types.Attributes;
            internal class Consumer {
                [EntryPoint]
                internal static void Run() {
                    Shared.State value = Shared.State.Ready;
                    System.Console.WriteLine(value);
                }
            }
            """, "Consumer.client.cs");
        var compilation = Compile(declarations, consumer);
        var modules = new ModuleGraph(new Dictionary<string, ModuleLocation> {
            ["global::Shared.State"] = new(["ReplicatedStorage", "State"], false),
        });

        var module = Transpile(declarations, compilation, ScriptType.Module, modules);
        var script = Transpile(consumer, compilation, ScriptType.Local, modules);

        Assert.Multiple(() => {
            Assert.That(module, Does.Contain("export type State = number"));
            Assert.That(module, Does.Contain("State = State"));
            Assert.That(script, Does.Contain("""require("@game/ReplicatedStorage/State")"""));
            Assert.That(script, Does.Contain("State.Ready"));
            Assert.That(script, Does.Contain("State.__format(value)"));
        });
    }

    [Test]
    public void PreservesRobloxEnumItems() {
        var tree = Parse("""
            using RobloxCS.Types;
            internal class NativeEnums {
                internal Enums.Material Material = Enums.Material.Plastic;
            }
            """, "NativeEnums.cs");
        var output = Transpile(tree, Compile(tree), ScriptType.Module, ModuleGraph.Empty);

        Assert.Multiple(() => {
            Assert.That(output, Does.Contain("Enum.Material.Plastic"));
            Assert.That(output, Does.Contain("Enum.Material"));
            Assert.That(output, Does.Not.Contain("Material.__format"));
        });
    }

    [TestCase("enum TooWide : ulong { Value = 9007199254740993 }", "53 bits")]
    [TestCase("class Outer { public enum Inner { Value } }", "Nested enums")]
    [TestCase("enum Bits : long { One = 1 } class Usage { Bits Invert(Bits value) => ~value; }", "64-bit enums")]
    [TestCase("[System.Flags] enum Bits : long { One = 1 } class Usage { string Format(Bits value) => value.ToString(); }", "64-bit flags")]
    [TestCase("class Usage { System.DayOfWeek Value = System.DayOfWeek.Friday; }", "not declared in a transpiled module")]
    public void ReportsUnsupportedEnums(string source, string message) {
        var tree = Parse(source, "Unsupported.cs");
        var compilation = Compile(tree);
        var result = new CSharpTranspiler(new TranspilerOptions(ScriptType.Module), new CSharpCompiler(tree, compilation, [])).Transpile();

        Assert.Multiple(() => {
            Assert.That(result.Ok, Is.False);
            Assert.That(result.Diagnostics.Select(diagnostic => diagnostic.Render()), Has.Some.Contains(message));
        });
    }

    private static SyntaxTree Parse(string source, string path) =>
        CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview), path);

    private CSharpCompilation Compile(params SyntaxTree[] trees) {
        var compilation = CSharpCompilation.Create("EnumTests", trees, _references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        Assert.That(compilation.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error), Is.Empty);

        return compilation;
    }

    private static string Transpile(SyntaxTree tree, CSharpCompilation compilation, ScriptType scriptType, ModuleGraph modules) {
        var result = new CSharpTranspiler(new TranspilerOptions(scriptType) { Modules = modules }, new CSharpCompiler(tree, compilation, [])).Transpile();
        Assert.That(result.Ok, Is.True, string.Join("\n", result.Diagnostics.Select(diagnostic => diagnostic.Render())));

        return new RendererWalker().Render(result.Chunk!);
    }
}
