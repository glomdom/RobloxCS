using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class BooleanLogic {
    private static bool Probe(string label, bool value) {
        Console.WriteLine(label);
        return value;
    }

    [EntryPoint]
    internal static void Run() {
        Console.WriteLine(false);
        Console.WriteLine(true);
        Console.WriteLine(!true);
        Console.WriteLine(Probe("skip-and", false) && Probe("bad", true));
        Console.WriteLine(Probe("skip-or", true) || Probe("bad", false));
        Console.WriteLine(Probe("take-and", true) && Probe("right", false));
        Console.WriteLine(Probe("take-or", false) || Probe("right", true));
        Console.WriteLine(Probe("eager-left", false) & Probe("eager-right", true));
        Console.WriteLine(Probe("eager-left", true) | Probe("eager-right", false));
        Console.WriteLine(true ^ false);
        Console.WriteLine("bool: " + false);
    }
}