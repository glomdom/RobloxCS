using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class RecursionAndReturns {
    private static int Factorial(int value) {
        if (value <= 1) {
            return 1;
        }

        return value * Factorial(value - 1);
    }

    private static int Fibonacci(int value) {
        if (value < 2) {
            return value;
        }

        return Fibonacci(value - 1) + Fibonacci(value - 2);
    }

    private static int Find(int value) {
        for (var i = 0; i < 6; i++) {
            if (i == value) {
                return i * 2;
            }
        }

        return -1;
    }

    [EntryPoint]
    internal static void Run() {
        Console.WriteLine(Factorial(0));
        Console.WriteLine(Factorial(6));
        Console.WriteLine(Fibonacci(8));
        Console.WriteLine(Find(0));
        Console.WriteLine(Find(4));
        Console.WriteLine(Find(9));
    }
}