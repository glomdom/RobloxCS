using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class NullValues {
    private static int Next() {
        Console.WriteLine("fallback evaluated");

        return 5;
    }

    [EntryPoint]
    internal static void Run() {
        string? missing = null;
        string? empty = "";

        Console.WriteLine(missing ?? "fallback");
        Console.WriteLine(empty ?? "wrong");
        Console.WriteLine("a" + missing + "b");
        Console.WriteLine(missing);

        int? number = null;
        int? zero = 0;

        Console.WriteLine(number ?? 42);
        Console.WriteLine(zero ?? 42);
        Console.WriteLine((number ?? Next()) + (zero ?? Next()));
        Console.WriteLine(missing == null);
        Console.WriteLine(empty != null);
        Console.WriteLine(default(bool));
        Console.WriteLine(default(int));
    }
}