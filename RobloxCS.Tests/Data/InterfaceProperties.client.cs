using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal interface IState {
    int Value { get; set; }
    string Label { get; }
}

internal class AutoState : IState {
    public int Value { get; set; } = 5;
    public string Label { get; } = "auto";
}

internal class ComputedState : IState {
    private int Storage;

    public int Value {
        get {
            Console.WriteLine("get");
            return Storage;
        }

        set {
            Console.WriteLine("set:" + value);
            Storage = value;
        }
    }

    public string Label => "computed";
}

internal class InterfaceProperties {
    private static IState Current = new AutoState();

    private static IState Fetch() {
        Console.WriteLine("fetch");

        return Current;
    }

    private static int Right() {
        Console.WriteLine("right");

        return 3;
    }

    private static void Use(IState item) {
        Console.WriteLine(item.Label);
        Console.WriteLine(item.Value);

        item.Value += 2;
        item.Value++;

        Console.WriteLine(item.Value);
    }

    [EntryPoint]
    internal static void Run() {
        Use(new AutoState());
        Use(new ComputedState());

        Current = new ComputedState();
        Fetch().Value += Right();

        Console.WriteLine(Current.Value);

        IState? missing = null;

        Console.WriteLine(missing?.Value ?? 99);

        missing = Current;

        Console.WriteLine(missing?.Value ?? 99);
    }
}