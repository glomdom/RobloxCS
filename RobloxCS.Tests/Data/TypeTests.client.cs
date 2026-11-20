using RobloxCS.Types;
using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal interface ITypeTestNamed {
    string Name { get; }
}

internal class TypeTestAnimal {
    internal virtual string Sound() => "...";
}

internal class TypeTestDog : TypeTestAnimal, ITypeTestNamed {
    public string Name => "rex";

    internal override string Sound() => "woof";
}

internal class TypeTestCat : TypeTestAnimal { }

internal enum TypeTestState {
    Idle,
    Running,
    Stopped,
}

internal class TypeTests {
    private static int Calls;

    private static object Next() {
        Calls++;

        return 4;
    }

    private static string Describe(object value) {
        if (value is int number) {
            return "int " + (number + 1);
        } else if (value is string text) {
            return "string " + text.Length;
        } else if (value is TypeTestDog dog) {
            return "dog " + dog.Sound();
        } else if (value is null) {
            return "null";
        }

        return "other";
    }

    private static string Bark(TypeTestAnimal animal) {
        if (animal is not TypeTestDog dog) {
            return "not a dog";
        }

        return dog.Name;
    }

    [EntryPoint]
    internal static void Run() {
        object number = 3;
        object text = "hello";
        TypeTestAnimal dog = new TypeTestDog();
        TypeTestAnimal cat = new TypeTestCat();
        object? nothing = null;

        Console.WriteLine(number is int);
        Console.WriteLine(number is string);
        Console.WriteLine(text is string);
        Console.WriteLine(dog is TypeTestDog);
        Console.WriteLine(cat is TypeTestDog);
        Console.WriteLine(dog is ITypeTestNamed);
        Console.WriteLine(cat is ITypeTestNamed);
        Console.WriteLine(nothing is int);
        Console.WriteLine(nothing is null);
        Console.WriteLine(dog is not null);

        Console.WriteLine(Describe(number));
        Console.WriteLine(Describe(text));
        Console.WriteLine(Describe(dog));
        Console.WriteLine(Describe(nothing!));
        Console.WriteLine(Describe(2.5));

        Console.WriteLine(Bark(dog));
        Console.WriteLine(Bark(cat));

        var state = TypeTestState.Running;

        Console.WriteLine(state is TypeTestState.Running);
        Console.WriteLine(state is TypeTestState.Idle or TypeTestState.Stopped);

        var count = 7;

        Console.WriteLine(count is > 5 and < 10);
        Console.WriteLine(count is < 5 or > 8);

        var enabled = true;
        if (enabled && number is int value) {
            Console.WriteLine(value * 10);
        }

        object[] items = [1, 2, 3, "stop", 4];
        var index = 0;
        var sum = 0;

        while (index < items.Length && items[index] is int item) {
            sum += item;
            index++;
        }

        Console.WriteLine(sum);

        if (Next() is var result && result is int four) {
            Console.WriteLine(four);
        }

        Console.WriteLine(Calls);

        var raw = new LuaTuple(9, "nine").Get<object>(0);

        Console.WriteLine(raw is int);
        Console.WriteLine(raw is int rawNumber ? rawNumber + 1 : -1);
    }
}