using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class Arithmetic {
    [EntryPoint]
    internal static void Run() {
        var a = 17;
        var b = 5;

        Console.WriteLine(a + b * 3);
        Console.WriteLine((a + b) * 3);
        Console.WriteLine(a - (b - 2));
        Console.WriteLine(a % b);
        Console.WriteLine(-a % b);
        Console.WriteLine(a % -b);
        Console.WriteLine(-a % -b);
        Console.WriteLine(7.0 / 2.0);
        Console.WriteLine(-(-a));

        a += 3;
        a *= 2;
        a -= 7;
        a %= 6;

        Console.WriteLine(a);
        Console.WriteLine(a >= 0);
        Console.WriteLine(a <= 5);
    }
}