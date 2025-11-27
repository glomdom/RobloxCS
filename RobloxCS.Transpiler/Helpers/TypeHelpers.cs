using RobloxCS.AST.Statements;
using RobloxCS.AST.Types;

namespace RobloxCS.Transpiler.Helpers;

public static class TypeHelpers {
    public static TableTypeInfo EmptyTableType() => new() { Fields = [] };

    public static void AddFieldToTable(TableTypeInfo table, TypeField field) {
        table.Fields.Add(field);
        field.Parent = table;
    }

    public static void AddFieldsToTable(TableTypeInfo table, params TypeField[] fields) => table.Fields.AddRange(fields);
    public static void AddFieldsToTable(TableTypeInfo table, List<TypeField> fields) => table.Fields.AddRange(fields);

    public static void AddFieldToKnownTableType(TypeDeclarationStatement stmt, TypeField field) {
        var tableInfo = (TableTypeInfo)stmt.DeclareAs;

        tableInfo.Fields.Add(field);
        field.Parent = tableInfo;
    }

    public static NameTypeFieldKey NameTypeFieldKeyFromString(string name) => new() { Name = name };

    public static TypeField FullTypeField(string keyName, TypeInfo value, AccessModifier? access = null) {
        var key = NameTypeFieldKeyFromString(keyName);

        var field = new TypeField { Key = key, Value = value, Access = access };
        key.Parent = field;
        value.Parent = field;

        return field;
    }

    public static TypeArgument FullTypeArgument(string name, TypeInfo info) {
        var type = new TypeArgument {
            Name = name,
            TypeInfo = info,
        };

        info.Parent = type;

        return type;
    }

    public static CallbackTypeInfo NoParamCallbackType(TypeInfo returnType) {
        var type = new CallbackTypeInfo {
            Arguments = [],
            ReturnType = returnType,
        };

        returnType.Parent = type;

        return type;
    }

    public static CallbackTypeInfo FullCallbackType(List<TypeArgument> args, TypeInfo returnType) {
        var type = new CallbackTypeInfo {
            Arguments = args,
            ReturnType = returnType,
        };

        args.ForEach(a => a.Parent = type);
        returnType.Parent = type;

        return type;
    }
}