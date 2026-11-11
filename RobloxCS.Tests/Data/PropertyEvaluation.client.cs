using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class PropertyEvaluation {
    private static PropertyEvaluation Current = null!;
    private static int Gets;
    private static int Sets;
    private static int Fetches;
    private int Storage = 10;

    internal int Value {
        get {
            Gets++;
            Console.WriteLine("get");

            return Storage;
        }

        set {
            Sets++;

            Console.WriteLine("set:" + value);

            Storage = value;
        }
    }

    private static PropertyEvaluation Fetch() {
        Fetches++;

        Console.WriteLine("fetch");

        return Current;
    }

    private static int Right() {
        Console.WriteLine("right");

        return 3;
    }

    [EntryPoint]
    internal static void Run() {
        Current = new PropertyEvaluation();

        Fetch().Value += Right();
        Console.WriteLine(Current.Value);

        Fetch().Value++;
        Console.WriteLine(Current.Value);

        Fetch().Value = Right();
        Console.WriteLine(Current.Value);
        Console.WriteLine(Gets);
        Console.WriteLine(Sets);
        Console.WriteLine(Fetches);

        Fetch().Value = (int)(PropertyInput)Right();
        Fetch().Storage = (int)(PropertyInput)Right();
        Fetch().Storage += (int)(PropertyInput)Right();
        Console.WriteLine(Current.Storage);
    }
}

internal enum PropertyInput {
    Zero
}