using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class CollectionCasts {
    [EntryPoint]
    internal static void Run() {
        string[] strings = ["a", "b"];
        object array = strings;

        Console.WriteLine((array as string[]) != null);
        Console.WriteLine((array as object[]) != null);
        Console.WriteLine((array as int[]) == null);
        Console.WriteLine((array as List<string>) == null);
        Console.WriteLine((array as IList<object>) != null);

        object[] objects = strings;
        objects[0] = "changed";

        Console.WriteLine(strings[0]);

        object list = new List<string> { "x" };

        Console.WriteLine((list as List<string>) != null);
        Console.WriteLine((list as List<object>) == null);
        Console.WriteLine((list as IList<object>) == null);
        Console.WriteLine((list as IReadOnlyList<object>) != null);
        Console.WriteLine((list as IEnumerable<object>) != null);
        Console.WriteLine((list as string[]) == null);

        object ints = new[] { 1, 2 };

        Console.WriteLine((ints as object[]) == null);
        Console.WriteLine((ints as IEnumerable<object>) == null);

        object? nothing = null;

        Console.WriteLine((nothing as int[]) == null);

        var nullable = nothing as List<int>;

        Console.WriteLine(nullable?.Count ?? -1);

        var cast = (string[])array;

        Console.WriteLine(cast[1]);
    }
}