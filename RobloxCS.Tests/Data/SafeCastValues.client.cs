using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal enum CastState {
    None,
    Ready,
}

internal class CastPlainObject;

internal class CastFormattedObject {
    public override string ToString() {
        return "custom";
    }
}

internal class SafeCastValues {
    [EntryPoint]
    internal static void Run() {
        object integer = 0;
        object number = 0.0;
        object boolean = false;
        object character = 'x';
        object text = "x";
        object state = CastState.Ready;

        Console.WriteLine(integer as int? ?? -1);
        Console.WriteLine(number as int? ?? -1);
        Console.WriteLine(integer as double? ?? -1);
        Console.WriteLine(number as double? ?? -1);
        Console.WriteLine(boolean as bool? ?? true);
        Console.WriteLine(character as char? ?? '?');
        Console.WriteLine(character as string == null);
        Console.WriteLine(text as string);
        Console.WriteLine(state as CastState? ?? CastState.None);
        Console.WriteLine(state as int? ?? -1);
        Console.WriteLine(integer as CastState? ?? CastState.Ready);
        Console.WriteLine((integer as object) == integer);
        Console.WriteLine((int)integer);

        int? nullable = 0;
        object? boxedNullable = nullable;

        Console.WriteLine(boxedNullable as int? ?? -1);

        nullable = null;
        boxedNullable = nullable;

        Console.WriteLine(boxedNullable == null);
        Console.WriteLine(boxedNullable as int? ?? -1);
        Console.WriteLine(5 as int? ?? -1);

        object small = (byte)5;
        Console.WriteLine(small as int? ?? -1);
        Console.WriteLine(small as byte? ?? 99);
        Console.WriteLine(integer);
        Console.WriteLine(boolean);
        Console.WriteLine(character);
        Console.WriteLine(state);
        Console.WriteLine(new CastPlainObject() as object);
        Console.WriteLine(new CastFormattedObject() as object);
        Console.WriteLine((integer as ValueType) != null);
        Console.WriteLine((integer as Enum) == null);
        Console.WriteLine((state as Enum) != null);
        Console.WriteLine((text as ValueType) == null);
    }
}