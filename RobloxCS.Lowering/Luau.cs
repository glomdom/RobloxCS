using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Functions;
using RobloxCS.AST.Parameters;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.AST.Types;

namespace RobloxCS.Lowering;

internal static class Luau {
    public static SymbolExpression Name(string name) => SymbolExpression.FromString(name);
    public static SymbolExpression Nil() => Name("nil");
    public static StringExpression Str(string value) => StringExpression.FromString(value);
    public static NumberExpression Num(double value) => NumberExpression.From(value);
    public static BooleanExpression Bool(bool value) => new() { Value = value };
    public static TableConstructorExpression Table() => TableConstructorExpression.Empty();
    public static TypeAssertionExpression Cast(Expression expression, TypeInfo type) => TypeAssertionExpression.From(expression, type);
    public static VarExpression Index(string obj, string field) => Index(Name(obj), field);

    public static VarExpression Index(Expression obj, string field) {
        return new VarExpression {
            Prefix = PrefixOf(obj),
            Suffixes = [
                new Dot {
                    Name = Name(field),
                },
            ],
        };
    }

    public static FunctionCallExpression Call(string function, params IEnumerable<Expression> args) {
        return new FunctionCallExpression {
            Prefix = NamePrefix.FromString(function),
            Suffixes = [AnonymousCall.FromArgs(Args(args))],
        };
    }

    public static FunctionCallExpression Call(VarExpression function, params IEnumerable<Expression> args) {
        return new FunctionCallExpression {
            Prefix = function.Prefix,
            Suffixes = [.. function.Suffixes, AnonymousCall.FromArgs(Args(args))],
        };
    }

    public static FunctionCallExpression MethodCall(Expression obj, string method, params IEnumerable<Expression> args) {
        return new FunctionCallExpression {
            Prefix = PrefixOf(obj),
            Suffixes = [
                new MethodCall {
                    Name = method,
                    Args = Args(args),
                },
            ],
        };
    }

    public static FunctionCallStatement Stmt(FunctionCallExpression call) => FunctionCallStatement.FromExpression(call);

    public static AssignmentStatement Assign(Var target, Expression value) {
        return new AssignmentStatement {
            Vars = [target],
            Expressions = [value],
        };
    }

    public static LocalAssignmentStatement Local(string name, Expression value) => LocalAssignmentStatement.Untyped(name, value);

    public static LocalAssignmentStatement Local(string name, TypeInfo type, Expression? value) {
        return value is null ? LocalAssignmentStatement.Naked(name, type) : LocalAssignmentStatement.Single(name, value, type);
    }

    public static ReturnStatement Return(params IEnumerable<Expression> values) => ReturnStatement.FromExpressions([.. values]);

    public static BasicTypeInfo Type(string name) => BasicTypeInfo.FromString(name);
    public static BasicTypeInfo VoidType() => BasicTypeInfo.Void();

    public static TypeInfo Optional(TypeInfo type) => type is BasicTypeInfo basic && !basic.Name.EndsWith('?') ? Type($"{basic.Name}?") : type;
    public static TypeofTypeInfo Typeof(Expression expression) => new() { Expression = expression };

    public static TableTypeInfo TableType(IEnumerable<(string Name, TypeInfo Type)> fields) {
        return new TableTypeInfo {
            Fields = [
                .. fields.Select(field => new TypeField {
                    Key = NameTypeFieldKey.FromString(field.Name),
                    Value = field.Type,
                }),
            ],
        };
    }

    public static TypeDeclarationStatement TypeDecl(string name, TypeInfo type, bool exported = false) {
        return new TypeDeclarationStatement {
            Name = name,
            DeclareAs = type,
            Exported = exported,
        };
    }

    public static FunctionDeclarationStatement Function(
        string owner,
        string name,
        List<(string Name, TypeInfo Type)> parameters,
        TypeInfo returnType,
        List<Statement> body
    ) {
        return new FunctionDeclarationStatement {
            Name = new FunctionName {
                Names = [owner, name],
            },
            Body = new FunctionBody {
                Parameters = [.. parameters.Select(parameter => (Parameter)NameParameter.FromString(parameter.Name))],
                TypeSpecifiers = [.. parameters.Select(parameter => parameter.Type)],
                ReturnType = returnType,
                Body = new Block {
                    Statements = body,
                },
            },
        };
    }
    
    public static AnonymousFunctionExpression Lambda(List<(string Name, TypeInfo Type)> parameters, TypeInfo returnType, List<Statement> body) {
        return new AnonymousFunctionExpression {
            Body = new FunctionBody {
                Parameters = [.. parameters.Select(parameter => (Parameter)NameParameter.FromString(parameter.Name))],
                TypeSpecifiers = [.. parameters.Select(parameter => parameter.Type)],
                ReturnType = returnType,
                Body = new Block {
                    Statements = body,
                },
            },
        };
    }

    private static FunctionArgs Args(IEnumerable<Expression> args) => new() { Arguments = [.. args] };

    private static Prefix PrefixOf(Expression expression) {
        if (expression is SymbolExpression symbol) {
            return NamePrefix.FromString(symbol.Value);
        }

        if (expression is not (VarExpression or FunctionCallExpression or ParenthesisExpression)) {
            expression = new ParenthesisExpression { Expression = expression };
        }

        return new ExpressionPrefix {
            Expression = expression,
        };
    }
}
