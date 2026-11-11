using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal enum OrderValue {
    Zero
}

internal class EvaluationOrder {
    private static int Counter;

    private static int Next(string label) {
        Counter++;
        Console.WriteLine(label + ":" + Counter);

        return Counter;
    }

    private static int Add(int first, int second) {
        return first * 10 + second;
    }

    [EntryPoint]
    internal static void Run() {
        Console.WriteLine(Add(Next("arg-left"), (int)(OrderValue)Next("arg-right")));
        Console.WriteLine(Next("binary-left") - (int)(OrderValue)Next("binary-right"));
        Console.WriteLine("concat:" + Next("concat-left") + (int)(OrderValue)Next("concat-right"));

        var value = 5;

        Console.WriteLine(Add(value, Set(8)));
        Console.WriteLine(Counter);
    }

    private static int Set(int value) {
        Counter = value;

        return value;
    }
}