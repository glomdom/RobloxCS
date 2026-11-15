using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class Arrays {
    private static int Sum(params int[] values) {
        var sum = 0;
        foreach (var value in values) sum += value;
        return sum;
    }

    [EntryPoint]
    internal static void Run() {
        var numbers = new int[4];

        Console.WriteLine(numbers.Length);
        Console.WriteLine(numbers[0]);

        numbers[0] = 12;
        numbers[3] = 5;
        numbers[0] += numbers[3];
        numbers[3]++;

        Console.WriteLine(numbers[0]);
        Console.WriteLine(numbers[3]);

        var flags = new bool[2];

        Console.WriteLine(flags[1]);

        var nullable = new string?[3];
        nullable[1] = "middle";

        Console.WriteLine(nullable.Length);

        foreach (var value in nullable) {
            Console.WriteLine(value ?? "null");
        }

        int[][] jagged = [[1, 2], [], [3]];

        Console.WriteLine(jagged.Length);
        Console.WriteLine(jagged[0][1]);
        Console.WriteLine(jagged[1].Length);
        Console.WriteLine(jagged[2][0]);

        var explicitArray = new[] { 7, 8, 9 };

        Console.WriteLine(explicitArray.GetLength(0));
        Console.WriteLine(explicitArray.GetLowerBound(0));
        Console.WriteLine(explicitArray.GetUpperBound(0));
        Console.WriteLine(explicitArray.Rank);
        Console.WriteLine(Array.Empty<int>() == Array.Empty<int>());
        Console.WriteLine(new int[0] == new int[0]);

        var chars = new char[1];

        Console.WriteLine(Sum(2, 3, 4));
        Console.WriteLine(Sum());

        foreach (object value in new[] { 5, 6 }) Console.WriteLine(value);
        foreach (int value in new object[] { 7, 8 }) Console.WriteLine(value);

        Console.WriteLine(chars[0] == '\0');
    }
}