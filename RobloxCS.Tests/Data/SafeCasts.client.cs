using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class CastBase(int value) {
    public int Value = value;

    public virtual int Read() {
        return Value;
    }
}

internal class CastDerived(int value) : CastBase(value) {
    public override int Read() {
        return Value + 10;
    }
}

internal class CastGrandchild(int value) : CastDerived(value);

internal class CastSibling(int value) : CastBase(value);

internal class CastSameShape {
    public int Value = 99;

    public int Read() {
        return Value;
    }
}

internal class CastHolder(object value) {
    public CastDerived? Value { get; } = value as CastDerived;
}

internal class SafeCasts {
    private static int Calls;
    private static int Remaining = 2;

    private static object? Fetch(object? value) {
        Calls++;

        return value;
    }

    private static object? Next(CastDerived value) {
        Calls++;
        if (Remaining == 0) {
            return null;
        }

        Remaining--;

        return value;
    }

    [EntryPoint]
    internal static void Run() {
        var original = new CastGrandchild(5);
        object value = original;
        var derived = value as CastDerived;
        var parent = value as CastBase;

        Console.WriteLine(derived?.Read() ?? -1);
        Console.WriteLine(parent?.Read() ?? -1);
        Console.WriteLine((value as CastDerived)!.Read());
        Console.WriteLine(derived == original);

        if (derived != null) {
            Console.WriteLine(derived.Read());
        }

        Console.WriteLine((value as CastGrandchild) == original);
        Console.WriteLine((value as CastSibling) == null);

        object sameShape = new CastSameShape();
        Console.WriteLine((sameShape as CastBase) == null);

        object plainBase = new CastBase(2);
        Console.WriteLine((plainBase as CastDerived) == null);
        Console.WriteLine((plainBase as CastBase)?.Read() ?? -1);

        object? missing = null;
        Console.WriteLine((missing as CastDerived) == null);
        Console.WriteLine((missing as string) == null);
        Console.WriteLine((missing as object) == null);
        Console.WriteLine(("text" as object) as string);
        Console.WriteLine((value as string) == null);
        Console.WriteLine(("text" as object as CastBase) == null);

        Console.WriteLine((Fetch(value) as CastDerived)?.Read() ?? -1);
        Console.WriteLine(Calls);
        Console.WriteLine(false && Fetch(value) as CastDerived != null);
        Console.WriteLine(true || Fetch(value) as CastDerived != null);
        Console.WriteLine(Calls);
        Console.WriteLine(false ? Fetch(value) as CastBase : Fetch(missing) as CastBase);
        Console.WriteLine(Calls);

        derived!.Value = 20;
        Console.WriteLine(original.Read());
        Console.WriteLine((Fetch(sameShape) as CastDerived)?.Read() ?? -1);
        Console.WriteLine(Calls);
        Console.WriteLine(new CastHolder(original).Value?.Read() ?? -1);
        Console.WriteLine(new CastHolder(sameShape).Value == null);

        var iterations = 0;
        while (Next(original) as CastDerived != null) {
            iterations++;
        }

        Console.WriteLine(iterations);
        Console.WriteLine(Calls);
    }
}