using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class CollectionLoops {
    [EntryPoint]
    internal static void Run() {
        List<int> numbers = [1, 2, 3, 4];
        var sum = 0;

        foreach (var number in numbers) {
            if (number == 2) continue;
            if (number == 4) break;

            for (var i = 0; i < 3; i++) {
                if (i == 1) continue;

                sum += number;
            }
        }

        Console.WriteLine(sum);

        int[] array = [1, 2, 3];

        foreach (var number in array) {
            array[2] = 9;

            Console.WriteLine(number);
        }

        for (var i = 0; i < 2; i++) {
            foreach (var number in numbers) {
                if (number < 4) continue;

                Console.WriteLine(i + number);
            }
        }

        List<int[]> nested = [[1], [], [2, 3]];

        foreach (var inner in nested) {
            foreach (var value in inner) {
                Console.WriteLine(value);
            }
        }
    }
}