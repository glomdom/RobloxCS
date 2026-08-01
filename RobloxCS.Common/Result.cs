using System.Diagnostics.CodeAnalysis;
using RobloxCS.Common.Diagnostics;

namespace RobloxCS.Common;

public sealed record Result<T> where T : notnull {
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Diagnostic))]
    public bool Ok { get; }

    public T? Value { get; }
    public Diagnostic? Diagnostic { get; }

    public Result(bool ok, T? value, Diagnostic? diagnostic) {
        Ok = ok;
        Value = value;
        Diagnostic = diagnostic;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Fail(Diagnostic diagnostic) => new(false, default, diagnostic);

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Diagnostic diagnostic) => Fail(diagnostic);
}