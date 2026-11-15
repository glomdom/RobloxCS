using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class Lists {
    [EntryPoint]
    internal static void Run() {
        var values = new List<string?>();

        Console.WriteLine(values.Count);
        Console.WriteLine(values.Capacity);

        values.Add(null);
        values.Add("a");
        values.Add(null);
        values.Insert(0, "first");
        values.Insert(values.Count, "last");

        Console.WriteLine(values.Count);
        Console.WriteLine(values.Capacity);
        Console.WriteLine(values.Contains(null));
        Console.WriteLine(values.IndexOf(null));
        Console.WriteLine(values.LastIndexOf(null));
        Console.WriteLine(values.Remove(null));

        values.RemoveAt(0);
        values[0] = "changed";

        foreach (var value in values) {
            Console.WriteLine(value ?? "null");
        }

        var array = values.ToArray();
        var copy = new List<string?>(array);
        values.Clear();

        Console.WriteLine(values.Count);
        Console.WriteLine(values.Capacity);
        Console.WriteLine(array.Length);
        Console.WriteLine(copy.Count);

        copy.Reverse();
        copy.RemoveRange(1, 1);
        copy.AddRange(copy);
        copy.InsertRange(1, copy);
        foreach (var value in copy) {
            Console.WriteLine(value ?? "null");
        }

        copy.TrimExcess();

        Console.WriteLine(copy.Capacity);
        Console.WriteLine(copy.GetRange(1, 2).Count);

        var initialized = new List<int> { 1, 2, 3 };
        initialized.Capacity = 10;

        Console.WriteLine(initialized.EnsureCapacity(11));

        initialized[1] *= 4;

        Console.WriteLine(initialized[1]);
        Console.WriteLine(initialized.Remove(99));

        initialized.Capacity += 2;
        initialized.Capacity++;

        Console.WriteLine(initialized.Capacity);

        List<string?> text = [null, "value"];
        text[0] += 5;
        text[1] += false;

        Console.WriteLine(text[0]);
        Console.WriteLine(text[1]);
    }
}