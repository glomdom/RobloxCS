using Microsoft.CodeAnalysis;
using RobloxCS.Lowering;
using RobloxCS.Modules;
using RobloxCS.Renderer;

namespace RobloxCS.Tests;

[TestFixture]
public class PrimaryConstructorCompilation {
    [Test]
    public void ChainsPrimaryConstructorsAcrossModules() {
        var baseTree = TestCompiler.Parse("""
            namespace Shared;

            public class PrimaryBase(int value) {
                public virtual int Read() {
                    return value;
                }
            }
            """, "PrimaryBase.cs");
        var derivedTree = TestCompiler.Parse("""
            using Shared;

            public class PrimaryDerived(int value) : PrimaryBase(value + 1) {
                public override int Read() {
                    return base.Read() + value;
                }
            }
            """, "PrimaryDerived.cs");
        var compilation = TestCompiler.Compile(baseTree, derivedTree);
        var modules = new ModuleGraph(new Dictionary<string, ModuleLocation> {
            ["global::Shared.PrimaryBase"] = new(["ReplicatedStorage", "PrimaryBase"], false),
            ["global::PrimaryDerived"] = new(["ReplicatedStorage", "PrimaryDerived"], false),
        });

        var baseResult = TestCompiler.Transpile(baseTree, compilation, ScriptType.Module, modules);
        var derivedResult = TestCompiler.Transpile(derivedTree, compilation, ScriptType.Module, modules);

        Assert.Multiple(() => {
            Assert.That(baseResult.Ok, Is.True, string.Join("\n", baseResult.Diagnostics.Select(diagnostic => diagnostic.Render())));
            Assert.That(derivedResult.Ok, Is.True, string.Join("\n", derivedResult.Diagnostics.Select(diagnostic => diagnostic.Render())));
        });

        var baseOutput = new RendererWalker().Render(baseResult.Chunk!);
        var derivedOutput = new RendererWalker().Render(derivedResult.Chunk!);

        Assert.Multiple(() => {
            Assert.That(baseOutput, Does.Contain("function PrimaryBase.new(value: number)"));
            Assert.That(derivedOutput, Does.Contain("@game/ReplicatedStorage/PrimaryBase"));
            Assert.That(derivedOutput, Does.Contain("PrimaryBase.__init(self,"));
            Assert.That(derivedOutput, Does.Contain("PrimaryBase.Read("));
        });
    }

    [TestCase("class Multiple(int value) { public Multiple() : this(0) {} }", "Constructor overloads")]
    [TestCase("class Generic<T>(T value) {}", "Generic classes")]
    [TestCase("class Outer { class Nested(int value) {} }", "Nested types")]
    [TestCase("class ByReference(ref int value) {}", "ref, out and in")]
    public void ReportsExistingConstructorRestrictions(string source, string message) {
        var tree = TestCompiler.Parse(source);
        var compilation = TestCompiler.Compile(tree);
        Assert.That(compilation.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error), Is.Empty);

        var result = TestCompiler.Transpile(tree, compilation);

        Assert.Multiple(() => {
            Assert.That(result.Ok, Is.False);
            Assert.That(result.Diagnostics.Select(diagnostic => diagnostic.Message), Has.Some.Contains(message));
        });
    }
}