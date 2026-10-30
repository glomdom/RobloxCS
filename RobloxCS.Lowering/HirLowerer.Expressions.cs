using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.HIR;
using RobloxCS.HIR.Expressions;
using RobloxCS.Types;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private Expression LowerExpression(HirExpression expression) {
        switch (expression) {
            case HirLiteral literal: {
                return LowerLiteral(literal);
            }

            case HirLocalRef local: {
                return Name(_names.NameFor(local.Symbol));
            }

            case HirParameterRef parameter: {
                return Name(_names.NameFor(parameter.Symbol));
            }

            case HirThis: {
                return Name("self");
            }

            case HirFieldAccess field: {
                return LowerFieldAccess(field);
            }

            case HirCall call: {
                return LowerCall(call) ?? (Expression)Nil();
            }

            case HirObjectCreation creation: {
                if (TryLowerDataTypeCreation(creation, out var dataType)) {
                    return dataType;
                }
                
                var type = creation.Constructor.ContainingType;

                if (GetRobloxNative(type) is { } native) {
                    if (native.Kind == RobloxNativeType.Instance) {
                        return Call(Index("Instance", "new"), Str(native.Name));
                    }
                    
                    Unsupported($"Instancing roblox type '{type.ToDisplayString()}' is not supported yet.", creation.Location);
                }
                
                if (!IsTranspiledType(type)) {
                    Unsupported($"Creating '{type.ToDisplayString()}' is not supported yet.", creation.Location);

                    return Nil();
                }

                var args = creation.Arguments.Select(argument => LowerExpression(argument.Value)).ToList();

                Log.Verbose("Lowering object creation {ClassName}.new with {ArgumentCount} arguments", type.Name, args.Count);

                return Call(Index(ReferenceType(type, creation.Location), "new"), args);
            }
            
            case HirBinary binary: {
                return LowerBinary(binary);
            }

            case HirIncrementDecrement increment: {
                Unsupported("++ and -- can only be used as statements yet.", increment.Location);

                return Nil();
            }
            
            case HirConversion conversion: {
                return LowerConversion(conversion);
            }

            case HirPropertyAccess access: {
                return LowerPropertyAccess(access);
            }

            case HirLambda lambda: {
                return LowerLambda(lambda);
            }
            
            case HirUnary unary: {
                if (TryLowerDataTypeOperator(unary, out var dataTypeUnary)) {
                    return dataTypeUnary;
                }

                if (unary.OperatorMethod is not null) {
                    Unsupported($"User-defined operator '{unary.OperatorMethod.ToDisplayString()}' is not supported yet.", unary.Location);

                    return Nil();
                }

                UnOp? op = unary.Op switch {
                    UnaryOperatorKind.Minus => UnOp.Minus,
                    UnaryOperatorKind.Not => UnOp.Not,
                    _ => null,
                };

                if (op is null) {
                    if (unary.Op is not UnaryOperatorKind.Plus) {
                        Unsupported($"Unary operator '{unary.Op}' is not supported yet.", unary.Location);
                    }

                    return LowerExpression(unary.Operand);
                }

                return new UnaryOperatorExpression {
                    UnOp = op.Value,
                    Expression = LowerExpression(unary.Operand),
                };
            }

            default: {
                Unsupported($"Lowering '{expression.GetType().Name}' expressions are not supported yet.", expression.Location);

                return Nil();
            }
        }
    }

    private AnonymousFunctionExpression LowerLambda(HirLambda lambda) {
        Log.Verbose("Lowering lambda with {ParameterCount} parameters", lambda.Parameters.Length);

        var parameters = LowerParameters(lambda.Parameters);
        var returnType = LowerType(lambda.Symbol.ReturnType, lambda.Location);
        var outerReturnsSelf = _returnsSelf;

        _returnsSelf = false;

        var body = LowerBlock(lambda.Body);

        _returnsSelf = outerReturnsSelf;

        return Lambda(parameters, returnType, body);
    }

    private Expression LowerPropertyAccess(HirPropertyAccess access) {
        if (TryLowerDataTypeProperty(access, out var dataTypeProperty)) {
            return dataTypeProperty;
        }
        
        var property = access.Symbol;
        if (property.IsStatic && TryGetServiceName(property, out var service)) {
            return MethodCall(Name("game"), "GetService", Str(service));
        }
        
        var name = RobloxNameOf(property);

        if (!property.IsStatic) {
            return Index(access.Receiver is null ? Name("self") : LowerExpression(access.Receiver), name);
        }

        Unsupported($"Static property '{property.ToDisplayString()}' is not supported yet.", access.Location);

        return Nil();
    }
    
    private static bool TryGetServiceName(IPropertySymbol property, out string name) {
        var attribute = property.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "RobloxServiceAttribute");
        name = attribute?.ConstructorArguments[0].Value as string ?? string.Empty;

        return attribute is not null;
    }

    private static Expression LowerLiteral(HirLiteral literal) {
        return literal.Value switch {
            null => Nil(),
            bool value => Bool(value),
            string value => Str(value),
            char value => Str(value.ToString()),

            _ => Num(Convert.ToDouble(literal.Value, CultureInfo.InvariantCulture)),
        };
    }

    private VarExpression LowerFieldAccess(HirFieldAccess access) {
        var field = access.Symbol;
        if (field.IsStatic) {
            Log.Verbose("Lowering static field access {ClassName}.{FieldName}", field.ContainingType.Name, field.Name);

            return Index(ReferenceType(field.ContainingType, access.Location), field.Name);
        }

        var receiver = access.Receiver is null ? Name("self") : LowerExpression(access.Receiver);

        return Index(receiver, field.Name);
    }

    private FunctionCallExpression? LowerCall(HirCall call) {
        if (TryLowerDataTypeCall(call, out var dataTypeCall)) {
            return dataTypeCall;
        }
        
        var method = call.Method;
        var args = call.Arguments.Select(argument => LowerExpression(argument.Value)).ToList();

        if (IsConsoleWrite(method)) {
            if (args.Count > 1) {
                Unsupported("Console format strings are not supported yet.", call.Location);

                return null;
            }

            Log.Verbose("Rewriting {Method} to print", method.ToDisplayString());

            return Call("print", args);
        }

        if (GetRobloxNative(method.ContainingType) is { } native) {
            return LowerNativeCall(call, native, args) as FunctionCallExpression;
        }
        
        if (!IsTranspiledType(method.ContainingType)) {
            Unsupported($"Calls to '{method.ToDisplayString()}' are not supported yet.", call.Location);

            return null;
        }

        if (method.IsStatic) {
            Log.Verbose("Lowering static call {ClassName}.{MethodName} with {ArgumentCount} arguments", method.ContainingType.Name, method.Name, args.Count);

            return Call(Index(ReferenceType(method.ContainingType, call.Location), method.Name), args);
        }

        Log.Verbose(
            "Lowering method call {MethodName} on {Receiver} with {ArgumentCount} arguments",
            method.Name, call.Receiver is null ? "implicit self" : call.Receiver.GetType().Name, args.Count
        );

        var receiver = call.Receiver is null ? Name("self") : LowerExpression(call.Receiver);

        return MethodCall(receiver, method.Name, args);
    }
    
    private Expression LowerBinary(HirBinary binary) {
        if (TryLowerDataTypeOperator(binary, out var dataTypeBinary)) {
            return dataTypeBinary;
        }
        
        if (binary.OperatorMethod is not null) {
            Unsupported($"User-defined operator '{binary.OperatorMethod.ToDisplayString()}' is not supported yet.", binary.Location);

            return Nil();
        }

        if (binary.Op is BinaryOperatorKind.Add && binary.Type.SpecialType is SpecialType.System_String) {
            return LowerStringConcat(binary);
        }

        var left = LowerExpression(binary.Left);
        var right = LowerExpression(binary.Right);

        if (binary.Op is BinaryOperatorKind.Remainder) {
            Log.Verbose("Lowering % to math.fmod to keep C#'s truncated remainder");

            return Call(Index("math", "fmod"), left, right);
        }

        if (binary.Op is BinaryOperatorKind.Divide && IsIntegral(binary.Type)) {
            Unsupported("Integer division is not supported yet.", binary.Location);

            return Nil();
        }

        if (ToBinOp(binary.Op) is not { } op) {
            Unsupported($"Binary operator '{binary.Op}' is not supported yet.", binary.Location);

            return Nil();
        }

        return new BinaryOperatorExpression {
            Left = left,
            Op = op,
            Right = right,
        };
    }
    
    private Expression LowerConversion(HirConversion conversion) {
        switch (conversion.Kind) {
            case HirConversionKind.Identity:
            case HirConversionKind.Boxing:
            case HirConversionKind.Unboxing:
            case HirConversionKind.Reference:
            case HirConversionKind.NullableWrap:
            case HirConversionKind.NullableUnwrap: {
                return LowerExpression(conversion.Operand);
            }

            case HirConversionKind.Numeric: {
                if (conversion.IsImplicit) {
                    return LowerExpression(conversion.Operand);
                }

                Unsupported("Explicit numeric casts are not supported yet.", conversion.Location);

                return Nil();
            }

            default: {
                Unsupported($"'{conversion.Kind}' conversions are not supported yet.", conversion.Location);

                return Nil();
            }
        }
    }
    
    private static BinOp? ToBinOp(BinaryOperatorKind kind) {
        return kind switch {
            BinaryOperatorKind.Add => BinOp.Plus,
            BinaryOperatorKind.Subtract => BinOp.Minus,
            BinaryOperatorKind.Multiply => BinOp.Star,
            BinaryOperatorKind.Divide => BinOp.Slash,
            BinaryOperatorKind.Equals => BinOp.TwoEqual,
            BinaryOperatorKind.NotEquals => BinOp.TildeEqual,
            BinaryOperatorKind.LessThan => BinOp.LessThan,
            BinaryOperatorKind.LessThanOrEqual => BinOp.LessThanEqual,
            BinaryOperatorKind.GreaterThan => BinOp.GreaterThan,
            BinaryOperatorKind.GreaterThanOrEqual => BinOp.GreaterThanEqual,
            BinaryOperatorKind.ConditionalAnd => BinOp.And,
            BinaryOperatorKind.ConditionalOr => BinOp.Or,

            _ => null,
        };
    }

    private static bool IsIntegral(ITypeSymbol type) {
        return type.SpecialType is SpecialType.System_SByte or SpecialType.System_Byte
            or SpecialType.System_Int16 or SpecialType.System_UInt16
            or SpecialType.System_Int32 or SpecialType.System_UInt32
            or SpecialType.System_Int64 or SpecialType.System_UInt64;
    }

    private Expression LowerStringConcat(HirBinary binary) {
        var operands = new List<HirExpression>();
        FlattenConcat(binary, operands);

        Log.Verbose("Lowering string concatenation of {OperandCount} operands to an interpolated string", operands.Count);

        var segments = new List<InterpolatedStringSegment>();
        var text = new StringBuilder();

        foreach (var operand in operands) {
            if (operand is HirLiteral { Value: string literal }) {
                text.Append(literal);

                continue;
            }

            segments.Add(new InterpolatedStringSegment {
                Literal = text.ToString(),
                Expression = LowerExpression(operand),
            });

            text.Clear();
        }

        if (segments.Count == 0) {
            return Str(text.ToString());
        }

        return new InterpolatedStringExpression {
            Segments = segments,
            LastString = text.ToString(),
        };
    }

    private static void FlattenConcat(HirExpression expression, List<HirExpression> operands) {
        while (true) {
            if (expression is HirBinary { Op: BinaryOperatorKind.Add, OperatorMethod: null, Type.SpecialType: SpecialType.System_String } binary) {
                FlattenConcat(binary.Left, operands);

                expression = binary.Right;
            
                continue;
            }

            operands.Add(expression);
            
            break;
        }
    }

    private static bool IsConsoleWrite(IMethodSymbol method) {
        return method is { Name: "WriteLine" or "Write" } && method.ContainingType.ToDisplayString() == "System.Console";
    }
}