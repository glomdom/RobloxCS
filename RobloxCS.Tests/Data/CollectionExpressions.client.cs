using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class CollectionExpressions {
    [EntryPoint]
    internal static void Run() {
        int[] empty = [];
        List<int> list = [1, 2, 3];
        int[] array = [0, ..list, 4, ..empty, ..new[] { 5, 6 }];

        foreach (var value in array) {
            Console.WriteLine(value);
        }

        string?[] holes = [null, "a", null];
        List<string?> copied = ["start", ..holes, "end", null];

        Console.WriteLine(copied.Count);

        foreach (var value in copied) {
            Console.WriteLine(value ?? "null");
        }

        object?[] boxed = [..new int?[] { 0, null, 2 }, false, "text"];
        foreach (var value in boxed) {
            Console.WriteLine(value ?? "null");
        }

        IEnumerable<int> enumerable = [8, 9];
        IReadOnlyList<int> readOnly = [..enumerable, 10];

        Console.WriteLine((enumerable as int[]) == null);
        Console.WriteLine((enumerable as List<int>) == null);
        Console.WriteLine(((ICollection<int>)enumerable).IsReadOnly);
        Console.WriteLine((readOnly as int[]) == null);

        IList<int> mutable = [..readOnly];
        mutable.Add(11);

        Console.WriteLine(mutable.Count);
        Console.WriteLine(mutable[3]);

        int[] characters = [..new[] { 'a', 'b' }];

        Console.WriteLine(characters[0]);
        Console.WriteLine(characters[1]);
    }
}