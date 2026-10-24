using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.Common.Diagnostics;
using RobloxCS.CompilerPipeline.Documents;
using RobloxCS.CompilerPipeline.Projects;
using RobloxCS.Lowering;
using RobloxCS.Modules;
using Serilog;

using Diagnostic = RobloxCS.Common.Diagnostics.Diagnostic;

namespace RobloxCS.CompilerPipeline.Modules;

public static class ModuleGraphBuilder {
    public static async Task<ModuleGraph> BuildAsync(IEnumerable<ProjectPlan> plans, List<Diagnostic> diagnostics, CancellationToken cancellation) {
        var types = new Dictionary<string, ModuleLocation>();

        foreach (var plan in plans) {
            foreach (var document in plan.Project.Documents.Where(Filters.ShouldCompile)) {
                if (await document.GetSyntaxTreeAsync(cancellation) is not { } tree) {
                    continue;
                }

                var model = plan.Compilation.GetSemanticModel(tree);
                var location = LocationOf(plan, document);

                foreach (var declaration in (await tree.GetRootAsync(cancellation)).DescendantNodes().OfType<TypeDeclarationSyntax>()) {
                    if (model.GetDeclaredSymbol(declaration, cancellation) is not { } symbol) {
                        continue;
                    }

                    var key = ModuleGraph.KeyOf(symbol);
                    if (!types.TryAdd(key, location)) {
                        diagnostics.Add(Diagnostic.Error(DiagnosticId.TypeSplitAcrossModules, $"'{key}' is declared in more than one file; partial types across files are not supported yet.", declaration.GetLocation()));

                        continue;
                    }

                    Log.Verbose("Mapped {Type} to {InstancePath}", key, string.Join('.', location.InstancePath));
                }
            }
        }

        Log.Debug("Built module graph with {TypeCount} types", types.Count);

        return new ModuleGraph(types);
    }

    private static ModuleLocation LocationOf(ProjectPlan plan, Document document) {
        var scriptType = ScriptTypeResolver.FromFileName(document.Name);
        var instanceName = StripScriptSuffix(Path.GetFileNameWithoutExtension(document.Name));

        return new ModuleLocation([.. plan.Anchor.InstancePath, .. document.Folders, instanceName], scriptType != ScriptType.Module);
    }

    private static string StripScriptSuffix(string stem) {
        foreach (var suffix in (string[])[".server", ".client"]) {
            if (stem.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)) {
                return stem[..^suffix.Length];
            }
        }

        return stem;
    }
}