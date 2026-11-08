using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class CompoundComputed {
    private int _gets;
    private int _sets;
    private int _fetches;

    internal int Score { get; set; }

    internal int Health {
        get {
            _gets++;

            return field;
        }
        set {
            _sets++;
            field = value;
        }
    } = 100;

    internal int Clamped {
        get;
        set => field = value > 10 ? 10 : value;
    }

    internal string Label { get; set; } = "hp";

    [EntryPoint]
    internal void Main() {
        var a = 10;
        a += 5;
        a -= 3;
        a *= 2;
        Console.WriteLine(a);

        var r = -7;
        r %= 3;
        Console.WriteLine(r);

        var d = 3.0;
        d /= 2;
        Console.WriteLine(d);

        var s = "a";
        s += "b";
        s += 1;
        Console.WriteLine(s);

        Score += 7;
        Score++;
        Console.WriteLine(Score);

        Health -= 30;
        Health++;
        Health--;
        Health *= 2;
        Console.WriteLine(Health);
        Console.WriteLine(_gets);
        Console.WriteLine(_sets);

        Clamped += 4;
        Clamped += 4;
        Clamped += 4;
        Console.WriteLine(Clamped);

        Label += "!";
        Console.WriteLine(Label);

        Self().Health += 5;
        Console.WriteLine(_fetches);
        Console.WriteLine(Health);

        Self().Score += 2;
        Console.WriteLine(_fetches);
        Console.WriteLine(Score);
    }

    private CompoundComputed Self() {
        _fetches++;

        return this;
    }
}