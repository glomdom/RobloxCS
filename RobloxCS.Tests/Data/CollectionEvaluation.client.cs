using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class CollectionEvaluation {
    private static int[] Current = [];
    private static int Calls;

    private static int[] GetArray() {
        Console.WriteLine("receiver");
        Calls++;

        return Current;
    }

    private static int GetIndex() {
        Console.WriteLine("index");
        Calls++;

        return 1;
    }

    private static int ChangeArray(int[] next) {
        Console.WriteLine("right");
        Calls++;
        Current = next;

        return 3;
    }

    private static int Next(int value) {
        Console.WriteLine(value);
        Calls++;

        return value;
    }

    private static int[] GetValues(int value) {
        Console.WriteLine(value);
        Calls++;

        return [value, value + 1];
    }

    [EntryPoint]
    internal static void Run() {
        int[] first = [10, 20];
        int[] second = [30, 40];
        Current = first;

        GetArray()[GetIndex()] += ChangeArray(second);

        Console.WriteLine(first[1]);
        Console.WriteLine(second[1]);
        Console.WriteLine(Calls);

        int[] values = [Next(1), ..GetValues(2), Next(4)];
        foreach (var value in values) Console.WriteLine(value);

        Console.WriteLine(Calls);

        List<int> initialized = new() { Next(5), Next(6) };
        initialized[GetIndex()] = Next(7);

        Console.WriteLine(initialized[1]);

        List<int> copy = new(GetValues(8));

        Console.WriteLine(copy[0]);

        int[] __collections = [9];

        Console.WriteLine(__collections[0]);

        int[] denominators = [-7];
        denominators[0] /= 2;

        Console.WriteLine(denominators[0]);

        denominators[0] %= 2;

        Console.WriteLine(denominators[0]);
    }
}