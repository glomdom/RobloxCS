using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class CallTarget(int value) {
    internal int Value = value;

    internal int Join(int first, int second) {
        return Value * 100 + first * 10 + second;
    }
}

internal class CallEvaluation {
    private static CallTarget Current = new(0);

    private static int Swap(CallTarget target, int result) {
        Current = target;
        return result;
    }

    private static CallTarget GetCurrent() {
        Console.WriteLine("receiver");
        return Current;
    }

    private static int Change(Func<int> action) {
        return action();
    }

    private static int[] Values(CallTarget target) {
        Current = target;
        return [2];
    }

    private static int ArrayCalls;
    private static int[] ArrayValues = [3, 1, 2];
    private static List<int> SearchValues = [1, 2];

    private static int[] GetArray() {
        ArrayCalls++;
        return ArrayValues;
    }

    private static int Start() {
        Console.WriteLine("start");
        return 0;
    }

    private static Predicate<int> GetPredicate() {
        SearchValues.Add(3);
        return value => value == 3;
    }

    [EntryPoint]
    internal static void Run() {
        var first = new CallTarget(1);
        var second = new CallTarget(2);
        Current = first;

        Console.WriteLine(Current.Join(second: Swap(second, 2), first: 1));

        Current = first;

        Console.WriteLine(GetCurrent().Join(1, Swap(second, 2)));

        Current = first;

        Console.WriteLine(Current.Join(1, Values(second)[0]));

        Current = first;

        Console.WriteLine(Current.Join(1, new[] { Swap(second, 2) }[0]));

        var value = 3;

        Console.WriteLine(first.Join(value, Change(() => {
            value = 8;

            return 2;
        })));

        Console.WriteLine(value);
        Console.WriteLine(first.Join(second: Change(() => {
            value = 9;

            return 2;
        }), first: value));

        Array.Sort(GetArray());

        Console.WriteLine(ArrayCalls);
        Console.WriteLine(ArrayValues[0]);

        Array.Clear(GetArray());

        Console.WriteLine(ArrayCalls);
        Console.WriteLine(ArrayValues[0]);
        Console.WriteLine(SearchValues.FindIndex(Start(), value => value == 2));
        Console.WriteLine(SearchValues.FindLastIndex(GetPredicate()));
    }
}