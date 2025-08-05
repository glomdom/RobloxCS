namespace RobloxCS.AST.Types;

public sealed class IntersectionTypeInfo : TypeInfo {
    public required List<TypeInfo> Types;

    public static IntersectionTypeInfo FromTypes(List<TypeInfo> types) => new() { Types = types };
    public static IntersectionTypeInfo FromInfos(params TypeInfo[] types) => new() { Types = [..types] };

    public static IntersectionTypeInfo FromNames(params string[] typeNames) => new() {
        Types = typeNames.Select(BasicTypeInfo.FromString).Cast<TypeInfo>().ToList()
    };
}