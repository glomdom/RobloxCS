using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class LoopBoundaries {
    [EntryPoint]
    internal static void Run() {
        var total = 0;
        for (var i = 0; i < 0; i++) {
            total += 100;
        }

        var counter = 0;
        while (counter < 5) {
            counter++;

            if (counter == 2) {
                continue;
            }

            total += counter;
            if (counter == 4) {
                break;
            }
        }

        Console.WriteLine(counter);
        Console.WriteLine(total);

        do {
            total++;
        } while (false);

        Console.WriteLine(total);

        for (var outer = 0; outer < 3; outer++) {
            for (var inner = 0; inner < 4; inner++) {
                if (inner == 1) {
                    continue;
                }

                if (inner == 3) {
                    break;
                }

                Console.WriteLine(outer * 10 + inner);
            }
        }

        var step = 0;
        for (var i = 0; i < 4; i++, step++) {
            if (i == 2) {
                continue;
            }

            Console.WriteLine(step);
        }

        Console.WriteLine(step);
    }
}