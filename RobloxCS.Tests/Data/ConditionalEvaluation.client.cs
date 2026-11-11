using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class ConditionalEvaluation {
    private static bool Choose(bool value) {
        Console.WriteLine("choose:" + value);

        return value;
    }

    private static int Value(string label, int value) {
        Console.WriteLine(label);

        return value;
    }

    [EntryPoint]
    internal static void Run() {
        Console.WriteLine(Choose(true) ? Value("yes", 1) : Value("wrong", 2));
        Console.WriteLine(Choose(false) ? Value("wrong", 3) : Value("no", 4));
        Console.WriteLine(Choose(true) ? Choose(false) ? Value("wrong", 5) : Value("nested", 6) : Value("wrong", 7));

        int? missing = null;
        int? present = 0;

        Console.WriteLine(missing ?? Value("fallback", 9));
        Console.WriteLine(present ?? Value("wrong", 10));
        Console.WriteLine(Choose(false) && (missing ?? Value("wrong", 11)) > 0);
        Console.WriteLine(Choose(true) || (missing ?? Value("wrong", 12)) > 0);
    }
}