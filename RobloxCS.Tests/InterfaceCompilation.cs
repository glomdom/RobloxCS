using Microsoft.CodeAnalysis;
using RobloxCS.HIR;
using RobloxCS.Lowering;
using RobloxCS.Modules;
using RobloxCS.Renderer;

namespace RobloxCS.Tests;

[TestFixture]
public class InterfaceCompilation {
    [Test]
    public void ImportsInterfaceTypesAndDispatchesAcrossModules() {
        var contractTree = TestCompiler.Parse("namespace Contracts; public interface ICounter { int Read(); int Value { get; set; } }", "Contract.cs");
        var implementationTree = TestCompiler.Parse("""
                                                    public class Counter : Contracts.ICounter {
                                                        public int Value { get; set; } = 7;
                                                        public int Read() => Value;
                                                    }
                                                    """, "Counter.cs");

        var consumerTree = TestCompiler.Parse("""
                                              using RobloxCS.Types.Attributes;
                                              internal class Consumer {
                                                  [EntryPoint]
                                                  internal static void Run() {
                                                      Contracts.ICounter counter = new Counter();
                                                      counter.Value += 3;
                                                      System.Console.WriteLine(counter.Read());
                                                  }
                                              }
                                              """, "Consumer.client.cs");

        var compilation = TestCompiler.Compile(contractTree, implementationTree, consumerTree);

        Assert.That(compilation.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error), Is.Empty);

        var modules = new ModuleGraph(new Dictionary<string, ModuleLocation> {
            ["global::Contracts.ICounter"] = new(["ReplicatedStorage", "Contract"], false),
            ["global::Counter"] = new(["ReplicatedStorage", "Counter"], false),
        });

        var contract = Render(contractTree, ScriptType.Module);
        var implementation = Render(implementationTree, ScriptType.Module);
        var consumer = Render(consumerTree, ScriptType.Local);
        var symbol = compilation.GetTypeByMetadataName("Contracts.ICounter")!;
        var readSlot = symbol.GetMembers("Read").OfType<IMethodSymbol>().Single().SlotName();

        using (Assert.EnterMultipleScope()) {
            Assert.That(contract, Does.Contain("export type __interfaceType_"));
            Assert.That(contract, Does.Contain(readSlot));
            Assert.That(contract, Does.Not.Contain("function "));
            Assert.That(implementation, Does.Contain("function Counter." + readSlot));
            Assert.That(consumer, Does.Contain("@game/ReplicatedStorage/Contract"));
            Assert.That(consumer, Does.Contain("@game/ReplicatedStorage/Counter"));
            Assert.That(consumer, Does.Contain(":" + readSlot + "("));
            Assert.That(consumer, Does.Not.Contain("local __interfaceType_"));
        }

        string Render(SyntaxTree tree, ScriptType scriptType) {
            var result = TestCompiler.Transpile(tree, compilation, scriptType, modules);
            Assert.That(result.Ok, Is.True, string.Join("\n", result.Diagnostics.Select(diagnostic => diagnostic.Render())));

            return new RendererWalker().Render(result.Chunk!);
        }
    }

    [Test]
    public void InterfaceImportsRespectClientVisibility() {
        var contractTree = TestCompiler.Parse("public interface IServerContract { int Read(); }", "Contract.cs");
        var consumerTree = TestCompiler.Parse("class Consumer { public int Use(IServerContract value) => value.Read(); }", "Consumer.client.cs");
        var compilation = TestCompiler.Compile(contractTree, consumerTree);
        var modules = new ModuleGraph(new Dictionary<string, ModuleLocation> {
            ["global::IServerContract"] = new(["ServerStorage", "Contract"], false),
        });

        var result = TestCompiler.Transpile(consumerTree, compilation, ScriptType.Local, modules);

        using (Assert.EnterMultipleScope()) {
            Assert.That(result.Ok, Is.False);
            Assert.That(result.Diagnostics.Select(diagnostic => diagnostic.Message), Has.Some.Contains("clients can't see"));
        }
    }
}