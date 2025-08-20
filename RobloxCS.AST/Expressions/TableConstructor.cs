namespace RobloxCS.AST.Expressions;

public sealed class TableConstructor : Expression {
    public required List<TableField> Fields { get; set; }

    public static TableConstructor Empty() {
        return new TableConstructor { Fields = [] };
    }

    public static TableConstructor With(params List<TableField> fields) {
        return new TableConstructor { Fields = fields };
    }

    public override TableConstructor DeepClone() => new() { Fields = Fields.Select(f => (TableField)f.DeepClone()).ToList() };
    public override void Accept(IAstVisitor v) => v.VisitTableConstructor(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitTableConstructor(this);

    public override IEnumerable<AstNode> Children() {
        return Fields;
    }
}

public abstract class TableField : AstNode;

public sealed class NoKey : TableField {
    public required Expression Expression { get; set; }

    public override NoKey DeepClone() => new() { Expression = (Expression)Expression.DeepClone() };
    public override void Accept(IAstVisitor v) => v.VisitNoKey(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitNoKey(this);

    public override IEnumerable<AstNode> Children() {
        yield return Expression;
    }
}

public sealed class NameKey : TableField {
    public required string Key { get; set; }
    public required Expression Value { get; set; }

    public override NameKey DeepClone() => new() { Key = Key, Value = (Expression)Value.DeepClone() };
    public override void Accept(IAstVisitor v) => v.VisitNameKey(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitNameKey(this);

    public override IEnumerable<AstNode> Children() {
        yield return Value;
    }
}

// TODO: Index signature