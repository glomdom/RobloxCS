using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class StringLength {
    private static string Name = "roblox";

    private static string Suffix() {
        return "cs";
    }

    [EntryPoint]
    internal static void Run() {
        var empty = "";

        Console.WriteLine(empty.Length);
        Console.WriteLine(Name.Length);
        Console.WriteLine((Name + Suffix()).Length);
        Console.WriteLine(Suffix().Length * 10);
        Console.WriteLine(-Name.Length);
    }
}
