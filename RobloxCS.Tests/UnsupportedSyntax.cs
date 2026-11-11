using Microsoft.CodeAnalysis;
using RobloxCS.Common.Diagnostics;

namespace RobloxCS.Tests;

[TestFixture]
public class UnsupportedSyntax {
    [TestCase("class Box<T> {}", "Generic classes")]
    [TestCase("class Outer { class Inner {} }", "Nested types")]
    [TestCase("class C { int F(int value) => value; int F(string value) => 0; }", "overloads")]
    [TestCase("class C { public C() {} public C(int value) {} }", "Constructor overloads")]
    [TestCase("class C { void F(ref int value) {} }", "ref, out and in")]
    [TestCase("class C { void F(params int[] values) {} }", "params")]
    [TestCase("class C { int[] F() => new int[] { 1, 2 }; }", "ArrayCreation")]
    [TestCase("class C { int F(int value) { switch (value) { case 0: return 1; default: return 2; } } }", "Switch")]
    [TestCase("class C { void F() { throw new System.Exception(); } }", "Throw")]
    [TestCase("class C { void F() { try {} catch (System.Exception) {} } }", "Try")]
    [TestCase("class C { void F(string value) { foreach (var item in value) {} } }", "ForEach")]
    [TestCase("class C { int F(int value) => value / 2; }", "Integer division")]
    [TestCase("class C { int F(double value) => (int)value; }", "numeric casts")]
    [TestCase("class C { int F() { int Local() => 3; return Local(); } }", "LocalFunction")]
    [TestCase("class C { string F(int value) => $\"value:{value}\"; }", "InterpolatedString")]
    [TestCase("interface IDefault { int Read() => 1; }", "Default interface implementations")]
    [TestCase("interface IGeneric<T> { T Read(); }", "Generic and nested interfaces")]
    [TestCase("class Outer { public interface INested {} }", "Generic and nested interfaces")]
    [TestCase("interface IStatic { static abstract int Read(); }", "Static interface members")]
    [TestCase("interface IIndexed { int this[int index] { get; } }", "indexers")]
    [TestCase("interface IEvents { event System.Action Changed; }", "events")]
    [TestCase("interface IRef { void Update(ref int value); }", "ref/params")]
    [TestCase("interface IParams { void Update(params int[] values); }", "ref/params")]
    [TestCase("interface IMethod { T Read<T>(); }", "generic methods")]
    [TestCase("interface IExternal : System.IDisposable {}", "external assemblies")]
    public void ReportsDiagnosticInsteadOfCrashing(string source, string message) {
        var tree = TestCompiler.Parse(source);
        var compilation = TestCompiler.Compile(tree);

        Assert.That(compilation.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error), Is.Empty);

        var result = TestCompiler.Transpile(tree, compilation);

        using (Assert.EnterMultipleScope()) {
            Assert.That(result.Ok, Is.False);
            Assert.That(result.Chunk, Is.Null);
            Assert.That(result.Diagnostics, Has.Some.Matches<RobloxCS.Common.Diagnostics.Diagnostic>(diagnostic =>
                diagnostic.Id == DiagnosticId.UnsupportedSyntax && diagnostic.Message.Contains(message, StringComparison.OrdinalIgnoreCase))
            );
        }
    }

    [TestCase("interface IContract { void Read(); } class C : IContract {}", "CS0535")]
    [TestCase("class C { int F() => missing; }", "CS0103")]
    [TestCase("class C { int F() => true; }", "CS0029")]
    [TestCase("class C { void F() { int value; System.Console.WriteLine(value); } }", "CS0165")]
    public void RejectsInvalidCSharpBeforeBuildingHir(string source, string diagnosticId) {
        var tree = TestCompiler.Parse(source);
        var result = TestCompiler.Transpile(tree, TestCompiler.Compile(tree));

        using (Assert.EnterMultipleScope()) {
            Assert.That(result.Ok, Is.False);
            Assert.That(result.Chunk, Is.Null);
            Assert.That(result.Diagnostics.Select(diagnostic => diagnostic.Id), Does.Contain(diagnosticId));
        }
    }
}