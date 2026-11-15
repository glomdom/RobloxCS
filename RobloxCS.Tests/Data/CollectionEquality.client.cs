using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class CollectionPoint(int value) {
    internal int Value = value;

    public override bool Equals(object? other) {
        var point = other as CollectionPoint;

        return point != null && Value == point.Value;
    }

    public override int GetHashCode() {
        return Value;
    }
}

internal class CollectionEquality {
    [EntryPoint]
    internal static void Run() {
        List<CollectionPoint?> points = [new(1), null, new(2), new(1)];

        Console.WriteLine(points.Contains(new(1)));
        Console.WriteLine(points.IndexOf(new(2)));
        Console.WriteLine(points.LastIndexOf(new(1)));
        Console.WriteLine(points.Remove(new(1)));
        Console.WriteLine(points.Remove(null));
        Console.WriteLine(points.Count);

        List<object?> objects = [0, false, 1.0, null, "text"];

        Console.WriteLine(objects.Contains(0));
        Console.WriteLine(objects.Contains(false));
        Console.WriteLine(objects.Contains(1));
        Console.WriteLine(objects.Contains(null));
        Console.WriteLine(objects.Contains("text"));

        var zero = 0.0;
        var nan = zero / zero;

        List<double> numbers = [nan, 0.0];

        Console.WriteLine(numbers.Contains(nan));

        List<object> boxed = [nan];

        Console.WriteLine(boxed.Contains(nan));

        string[][] jagged = [["a"], ["b"]];
        object array = jagged;

        Console.WriteLine((array as object[][]) != null);
    }
}