using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class MethodArguments {
    private static int Counter;

    private static int Next(string label) {
        Counter++;

        Console.WriteLine(label + ":" + Counter);

        return Counter;
    }

    private static int Join(int first, int second = 4) {
        return first * 10 + second;
    }

    private static int Add(int first, int second) {
        return first + second;
    }

    [EntryPoint]
    internal static void Run() {
        Console.WriteLine(Join(Next("first"), Next("second")));
        Console.WriteLine(Join(second: Next("named-second"), first: Next("named-first")));
        Console.WriteLine(Join(7));
        Console.WriteLine(Join(second: 8, first: 9));
        Console.WriteLine(Add(Next("nested"), Join(Next("inner-first"), Next("inner-second"))));
    }
}