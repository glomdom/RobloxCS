namespace RobloxCS.HIR;

// yanked from Microsoft.CodeAnalysis.CSharp.Conversion
public enum HirConversionKind {
    Identity,
    Numeric,
    Boxing,
    Unboxing,
    NullableWrap,
    NullableUnwrap,
    Reference,
    UserDefined,
}