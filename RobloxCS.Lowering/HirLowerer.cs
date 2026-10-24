using Microsoft.CodeAnalysis;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using RobloxCS.HIR;
using RobloxCS.Modules;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private const string EntryPointLocal = "__entrypointCls";

    public ScriptType ScriptType { get; }
    public DiagnosticBag Diagnostics { get; }
    public ModuleGraph Modules { get; }

    private readonly LoweringContext _names = new();
    private readonly HashSet<INamedTypeSymbol> _moduleTypes = new(SymbolEqualityComparer.Default);
    private readonly HashSet<INamedTypeSymbol> _imports = new(SymbolEqualityComparer.Default);

    public HirLowerer(ScriptType scriptType, ModuleGraph modules, DiagnosticBag diagnostics) {
        ScriptType = scriptType;
        Diagnostics = diagnostics;
        Modules = modules;
    }

    public Chunk Lower(HirModule module) {
        // TODO: prologue should require the runtime List:
        //   local List = require(game:GetService("ReplicatedStorage"):WaitForChild("robloxcs"):WaitForChild("RuntimeLib"):WaitForChild("List"))

        using var _ = LoggerSetup.PushPass("Lowering");

        Log.Information("Lowering {SourcePath} as a {ScriptType} script ({TypeCount} types)", module.SourcePath, ScriptType, module.Types.Length);

        foreach (var type in module.Types) {
            _moduleTypes.Add(type.Symbol);
        }

        var statements = new List<Statement>();
        foreach (var type in module.Types) {
            statements.AddRange(LowerTopLevelType(type));
        }

        statements.AddRange(LowerEntryPoint(module));
        
        if (ScriptType == ScriptType.Module) {
            statements.Add(Return(new TableConstructorExpression {
                Fields = [
                    ..module.Types
                        .Where(type => type.Symbol.TypeKind == TypeKind.Class)
                        .Select(TableField (type) => new NameKey { Key = ClassName(type.Symbol), Value = Name(ClassName(type.Symbol)) }),
                ],
            }));
        }

        Log.Information("Lowered {SourcePath} into {StatementCount} top-level statements", module.SourcePath, statements.Count);
        
        statements.InsertRange(0, LowerImports());
        
        return new Chunk {
            Mode = TypeCheckMode.Strict,
            Block = new Block {
                Statements = statements,
            },
        };
    }
    
    private List<Statement> LowerImports() {
        var statements = new List<Statement>();

        var byModule = _imports
            .Select(type => (Type: type, Module: Modules.TryGetModule(type, out var module) ? module : null!))
            .GroupBy(entry => string.Join('/', entry.Module.InstancePath), StringComparer.Ordinal)
            .OrderBy(group => group.Key, StringComparer.Ordinal);

        foreach (var group in byModule) {
            var path = group.First().Module.InstancePath;
            var moduleLocal = string.Join('_', path.Skip(1));

            statements.Add(Local(moduleLocal, Call("require", RequirePath(path))));

            foreach (var (type, _) in group.OrderBy(entry => entry.Type.Name, StringComparer.Ordinal)) {
                statements.Add(Local(type.Name, Index(moduleLocal, type.Name)));
                statements.Add(TypeDecl(type.Name, Type($"{moduleLocal}.{type.Name}")));
            }
        }

        return statements;
    }

    private static Expression RequirePath(IReadOnlyList<string> path) {
        Expression current = MethodCall(Name("game"), "GetService", Str(path[0]));

        foreach (var segment in path.Skip(1)) {
            current = MethodCall(current, "WaitForChild", Str(segment));
        }

        return current;
    }

    private List<Statement> LowerEntryPoint(HirModule module) {
        var entryPoints = module.Types
            .SelectMany(type => type.Methods.Where(method => method.IsEntryPoint).Select(method => (Type: type, Method: method)))
            .ToList();

        if (entryPoints.Count == 0) {
            Log.Debug("No entry point found, skipping entry point call");

            return [];
        }

        if (entryPoints.Count > 1) {
            foreach (var (_, method) in entryPoints.Skip(1)) {
                Unsupported("Only one [EntryPoint] per file is supported.", method.Location);
            }

            return [];
        }

        var (owner, entryPoint) = entryPoints[0];
        var className = ClassName(owner.Symbol);

        if (entryPoint.Parameters.Length > 0) {
            Unsupported("Entry points can't take parameters.", entryPoint.Location);

            return [];
        }

        if (entryPoint.IsStatic) {
            Log.Debug("Calling static entry point {ClassName}.{MethodName}", className, entryPoint.Symbol.Name);

            return [Stmt(Call(Index(className, entryPoint.Symbol.Name)))];
        }

        var constructor = owner.Methods.FirstOrDefault(method => method.IsConstructor);
        if (constructor is { Parameters.Length: > 0 }) {
            Unsupported("An instance entry point's class needs a parameterless constructor.", entryPoint.Location);

            return [];
        }

        Log.Debug("Instantiating {ClassName} into {Local} to call entry point {MethodName}", className, EntryPointLocal, entryPoint.Symbol.Name);

        return [
            Local(EntryPointLocal, Call(Index(className, "new"))),
            Stmt(MethodCall(Name(EntryPointLocal), entryPoint.Symbol.Name)),
        ];
    }
    
    private bool IsTranspiledType(ITypeSymbol type) {
        return type is INamedTypeSymbol named && (_moduleTypes.Contains(named) || Modules.TryGetModule(named, out _));
    }

    private string ReferenceType(INamedTypeSymbol type, Location location) {
        if (_moduleTypes.Contains(type) || !Modules.TryGetModule(type, out var module)) {
            return ClassName(type);
        }

        if (module.IsScript) {
            Diagnostics.Error(DiagnosticId.UnsupportedSyntax, $"'{type.Name}' is declared in a script and can't be required. Move it to a module file.", location);
        } else if (ScriptType == ScriptType.Local && module.InstancePath[0] is "ServerScriptService" or "ServerStorage") {
            Diagnostics.Error(DiagnosticId.UnsupportedSyntax, $"'{type.Name}' lives in {module.InstancePath[0]}, which clients can't see.", location);
        }

        if (_imports.Add(type)) {
            Log.Debug("Importing {Type} from {Module}", type.Name, string.Join('.', module.InstancePath));
        }

        return ClassName(type);
    }
    
    private static string ClassName(INamedTypeSymbol type) => type.Name;

    private void Unsupported(string message, Location location) {
        Log.Debug("Reporting unsupported construct: {Message}", message);

        Diagnostics.Error(DiagnosticId.UnsupportedSyntax, message, location);
    }
}