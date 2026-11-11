using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class DoWhileContinue {
    private static bool Check(int value) {
        Console.WriteLine("check:" + value);

        return value < 3;
    }

    [EntryPoint]
    internal static void Run() {
        var value = 0;
        do {
            value++;

            if (value == 2) {
                continue;
            }

            Console.WriteLine("body:" + value);
        } while (Check(value));

        Console.WriteLine(value);

        for (var outer = 0; outer < 2; outer++) {
            var inner = 0;

            do {
                inner++;
                if (inner == 1) {
                    continue;
                }

                Console.WriteLine(outer * 10 + inner);
            } while (inner < 2);
        }

        do {
            Console.WriteLine("break");

            break;
        } while (Check(99));
    }
}