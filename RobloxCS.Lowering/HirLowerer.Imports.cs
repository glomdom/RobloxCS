using System.Text;
using Microsoft.CodeAnalysis;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.Common.Diagnostics;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private List<Statement> LowerImports() {
        var statements = new List<Statement>();

        var byModule = _imports
            .Select(type => (Type: type, Module: Modules.TryGetModule(type, out var module) ? module : null!))
            .GroupBy(entry => string.Join('/', entry.Module.InstancePath), StringComparer.Ordinal)
            .OrderBy(group => group.Key, StringComparer.Ordinal);

        foreach (var group in byModule) {
            var path = group.First().Module.InstancePath;
            var moduleLocal = UniqueImportName(string.Join('_', path.Skip(1)));

            statements.Add(Local(moduleLocal, Call("require", RequirePath(path))));

            foreach (var (type, _) in group.OrderBy(entry => entry.Type.Name, StringComparer.Ordinal)) {
                var name = TypeName(type);
                if (type.TypeKind != TypeKind.Interface) {
                    statements.Add(Local(name, Index(moduleLocal, name)));
                }

                statements.Add(TypeDecl(name, Type($"{moduleLocal}.{name}")));
            }
        }

        return statements;
    }

    private static StringExpression RequirePath(IReadOnlyList<string> path) {
        return Str($"@game/{string.Join('/', path)}");
    }

    private bool IsTranspiledType(ITypeSymbol type) {
        return type is INamedTypeSymbol named && (_moduleTypes.Contains(named) || Modules.TryGetModule(named, out _));
    }

    private string ReferenceType(INamedTypeSymbol type, Location location) {
        if (_moduleTypes.Contains(type) || !Modules.TryGetModule(type, out var module)) {
            return TypeName(type);
        }

        if (module.IsScript) {
            Diagnostics.Error(DiagnosticId.UnsupportedSyntax, $"'{type.Name}' is declared in a script and can't be required. Move it to a module file.", location);
        } else if (ScriptType == ScriptType.Local && module.InstancePath[0] is "ServerScriptService" or "ServerStorage") {
            Diagnostics.Error(DiagnosticId.UnsupportedSyntax, $"'{type.Name}' lives in {module.InstancePath[0]}, which clients can't see.", location);
        }

        if (_imports.Add(type)) {
            Log.Debug("Importing {Type} from {Module}", type.Name, string.Join('.', module.InstancePath));
        }

        return TypeName(type);
    }

    private static string SafeIdentifier(string name) {
        var builder = new StringBuilder(name.Length);

        foreach (var character in name) {
            builder.Append(char.IsAsciiLetterOrDigit(character) || character == '_' ? character : '_');
        }

        if (builder.Length == 0 || char.IsAsciiDigit(builder[0])) {
            builder.Insert(0, '_');
        }

        return builder.ToString();
    }

    private string UniqueImportName(string raw) {
        var name = SafeIdentifier(raw);
        var candidate = name;

        for (var suffix = 2; !_importNames.Add(candidate); suffix++) {
            candidate = $"{name}{suffix}";
        }

        return candidate;
    }
}
