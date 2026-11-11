using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class Enums {
    private Direction Current;
    private static Direction Initial = (Direction)3;
    public Access Permissions { get; set; } = Access.Read;

    private static Access Fetch() {
        Console.WriteLine("fetch");
        return Access.Write;
    }

    private Direction Echo(Direction value) {
        return value;
    }

    [EntryPoint]
    internal void Run() {
        Console.WriteLine((int)Current);
        Current = Direction.West;
        Console.WriteLine((int)Echo(Current));
        Console.WriteLine((int)Direction.Alias);
        Console.WriteLine((int)Direction.Negative);
        Console.WriteLine((int)(Direction)42);
        Direction zero = 0;
        Console.WriteLine((int)zero);
        if (Direction.Alias == Direction.South) {
            Console.WriteLine("alias");
        }
        Direction? optional = Direction.East;
        Console.WriteLine((int)(optional ?? Direction.North));
        Permissions |= Fetch();
        Console.WriteLine((int)Permissions);
        Console.WriteLine((int)(Permissions & Access.Write));
        Console.WriteLine((int)(Permissions ^ Access.Read));
        Console.WriteLine((int)~Permissions);
        Console.WriteLine((int)~SignedBits.None);
        Console.WriteLine((int)(SignedBits.High | (SignedBits)1));
        var large = 258;
        Console.WriteLine((int)(Access)large);
        Console.WriteLine((int)(Direction)(-2.9));
        Console.WriteLine((long)Wide.Value);
        Console.WriteLine((int)default(Direction));
        Console.WriteLine(Direction.North);
        Console.WriteLine(Current.ToString());
        Console.WriteLine("direction: " + Direction.East);
        Console.WriteLine((Direction)42);
        Console.WriteLine(Access.All);
        Console.WriteLine(Permissions);
        Console.WriteLine(Access.None);
        Console.WriteLine((Access)128);
        Console.WriteLine(SignedBits.All);
        Console.WriteLine((int)Initial);
        Console.WriteLine((uint)~UnsignedBits.None);
        var value = (Direction)3;
        if (value <= Direction.West && value >= Direction.North) {
            Console.WriteLine("range");
        }
    }
}

internal enum Direction {
    North,
    East,
    South = 10,
    West,
    Alias = South,
    Negative = -2,
}

[Flags]
internal enum Access : byte {
    None = 0,
    Read = 1,
    Write = 2,
    Execute = 4,
    All = Read | Write | Execute,
}

internal enum SignedBits {
    None = 0,
    High = int.MinValue,
    All = -1,
}

internal enum Wide : long {
    Value = 5_000_000_000,
}

internal enum UnsignedBits : uint {
    None = 0,
    All = uint.MaxValue,
}
