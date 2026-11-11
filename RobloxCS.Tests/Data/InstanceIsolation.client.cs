using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class InstanceIsolation {
    private int Value;
    internal static int Total;
    internal InstanceIsolation(int value) { Value = value; Total += value; }
    internal int Add(int amount) { Value += amount; return Value; }
    internal bool Same(InstanceIsolation other) { return this == other; }
    internal void Reset() { Value = 0; return; }
}

internal class InstanceIsolationDriver {
    [EntryPoint]
    internal static void Run() {
        var first = new InstanceIsolation(3);
        var second = new InstanceIsolation(8);
        Console.WriteLine(first.Add(2));
        Console.WriteLine(second.Add(4));
        Console.WriteLine(first.Add(0));
        Console.WriteLine(first.Same(first));
        Console.WriteLine(first.Same(second));
        Console.WriteLine(InstanceIsolation.Total);
        first.Reset();
        Console.WriteLine(first.Add(1));
        Console.WriteLine(second.Add(0));
    }
}
