namespace RobloxCS.AST.Expressions;

public sealed class TableConstructor : Expression {
    public required List<TableField> Fields { get; set; }

    public static TableConstructor Empty() {
        return new TableConstructor { Fields = [] };
    }

    public static TableConstructor With(params List<TableField> fields) {
        return new TableConstructor { Fields = fields };
    }

    public override string ToString() => Fields.Count == 0 ? "{}" : $"{{ {string.Join(", ", Fields)} }}";
}

public abstract class TableField : AstNode;

public sealed class NoKey : TableField {
    public required Expression Expression { get; set; }

    public override string ToString() => Expression.ToString();
}

// TODO: Index signature