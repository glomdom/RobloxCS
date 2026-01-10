using RobloxCS.AST.Types;
using RobloxCS.Compiler;
using Serilog;

namespace RobloxCS.Transpiler.Semantics;

public sealed class GlobalRegistry {
    public readonly Dictionary<string, ClassInfo> Classes = [];

    public GlobalRegistry(CSharpCompiler.MetadataTypes types) {
        InitializeBuiltins(types);
    }

    public void RegisterClass(string name, string? parentName = null) {
        if (!Classes.ContainsKey(name)) {
            Classes[name] = new ClassInfo(name, parentName);
        }

        Log.Verbose("Registered class {ClassName} to global registry", name);
    }

    public void RegisterMember(string className, string memberName, TypeInfo type) {
        if (Classes.TryGetValue(className, out var classInfo)) {
            classInfo.AddMember(memberName, type);
        }

        Log.Verbose("Registered member {MemberName} to class {ClassName} in global registry", className, memberName);
    }

    public TypeInfo? GetMemberType(string className, string memberName) {
        if (!Classes.TryGetValue(className, out var info)) return null;

        var member = info.GetMember(memberName);
        if (member is not null) return member;

        return info.ParentName is not null ? GetMemberType(info.ParentName, memberName) : null;
    }

    private void InitializeBuiltins(CSharpCompiler.MetadataTypes types) {
        var list = new ClassInfo(types.ListTypeSymbol.MetadataName);
        list.AddMember("Count", BasicTypeInfo.Number());

        Classes["List"] = list;
    }
}