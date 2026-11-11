using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal interface IFirstView {
    int Read();
    int Value { get; set; }
}

internal interface ISecondView {
    int Read();
    int Value { get; set; }
}

internal class ExplicitViews : IFirstView, ISecondView {
    private int First = 1;
    private int Second = 2;

    int IFirstView.Read() {
        return First;
    }

    int ISecondView.Read() {
        return Second;
    }

    int IFirstView.Value {
        get => First;
        set => First = value;
    }

    int ISecondView.Value {
        get => Second;
        set => Second = value;
    }

    public int Read() {
        return 99;
    }
}

internal interface IOverloadedView {
    int Compute(int value);
    int Compute(string value);
}

internal class ExplicitOverloads : IOverloadedView {
    int IOverloadedView.Compute(int value) {
        return value + 1;
    }

    int IOverloadedView.Compute(string value) {
        return value == "yes" ? 10 : 20;
    }
}

internal interface IAutoExplicit {
    int Value { get; set; }
}

internal class AutoExplicit : IAutoExplicit {
    int IAutoExplicit.Value { get; set; } = 6;
}

internal class InterfaceExplicit {
    [EntryPoint]
    internal static void Run() {
        var item = new ExplicitViews();

        IFirstView first = item;
        ISecondView second = item;

        Console.WriteLine(first.Read());
        Console.WriteLine(second.Read());
        Console.WriteLine(item.Read());

        first.Value = 7;
        second.Value += 3;

        Console.WriteLine(first.Value);
        Console.WriteLine(second.Read());

        IOverloadedView overloaded = new ExplicitOverloads();

        Console.WriteLine(overloaded.Compute(4));
        Console.WriteLine(overloaded.Compute("yes"));

        IAutoExplicit auto = new AutoExplicit();

        Console.WriteLine(auto.Value);

        auto.Value = 12;

        Console.WriteLine(auto.Value);
    }
}