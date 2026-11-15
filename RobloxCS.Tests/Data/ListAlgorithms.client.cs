using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class ListAlgorithms {
    [EntryPoint]
    internal static void Run() {
        List<int> values = [4, 1, 3, 2, 0, 3];
        values.Sort();

        foreach (var value in values) Console.WriteLine(value);

        values.Sort((left, right) => right - left);

        foreach (var value in values) Console.WriteLine(value);

        Console.WriteLine(values.Exists(value => value == 3));
        Console.WriteLine(values.TrueForAll(value => value >= 0));
        var found = values.Find(value => value < 3);
        var last = values.FindLast(value => value > 1);
        Console.WriteLine(found);
        Console.WriteLine(last);
        Console.WriteLine(values.Find(value => value == 99));
        Console.WriteLine(values.FindIndex(value => value == 3));
        Console.WriteLine(values.FindIndex(2, 3, value => value == 3));
        Console.WriteLine(values.FindLastIndex(value => value == 3));
        Console.WriteLine(values.FindLastIndex(2, 3, value => value == 3));

        var selected = values.FindAll(value => value > 1);
        selected.ForEach(value => Console.WriteLine(value));

        var converted = values.ConvertAll(value => "item:" + value);

        Console.WriteLine(converted[0]);
        Console.WriteLine(converted.Capacity);
        Console.WriteLine(values.RemoveAll(value => value == 3));
        Console.WriteLine(values.RemoveAll(value => value == 99));

        values.ForEach(value => Console.WriteLine(value));

        List<string?> words = ["z", null, "a", null];
        words.Sort();

        foreach (var word in words) Console.WriteLine(word ?? "null");
        List<int> empty = [];

        Console.WriteLine(empty.TrueForAll(value => false));
        Console.WriteLine(empty.FindLastIndex(value => true));
        Console.WriteLine(empty.FindLast(value => true));
        Console.WriteLine(empty.RemoveAll(value => true));

        bool[] flags = [true, false, true];
        Array.Sort(flags);

        foreach (var flag in flags) Console.WriteLine(flag);
    }
}