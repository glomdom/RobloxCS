using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class CollectionCopies {
    [EntryPoint]
    internal static void Run() {
        int[] values = [1, 2, 3, 4, 5];

        Array.Copy(values, 0, values, 1, 4);

        foreach (var value in values) Console.WriteLine(value);

        Array.Copy(values, 1, values, 0, 4);
        Array.Clear(values, 1, 2);

        foreach (var value in values) Console.WriteLine(value);

        Array.Reverse(values);

        Console.WriteLine(Array.IndexOf(values, 0));
        Console.WriteLine(Array.LastIndexOf(values, 0));

        var copy = (int[])values.Clone();
        copy[0] = 99;

        Console.WriteLine(values[0]);
        Console.WriteLine(copy[0]);

        List<int> list = [7, 8, 9];
        list.CopyTo(values, 1);
        list.CopyTo(1, copy, 2, 2);

        foreach (var value in values) Console.WriteLine(value);
        foreach (var value in copy) Console.WriteLine(value);

        var constructed = new List<int>(new[] { 1, 2 });

        Console.WriteLine(constructed.Capacity);
        Console.WriteLine(list.ToArray());
        Console.WriteLine(list);

        Array.Clear(copy);

        Console.WriteLine(copy[0]);

        int[] empty = [];

        Console.WriteLine(empty == Array.Empty<int>());
    }
}