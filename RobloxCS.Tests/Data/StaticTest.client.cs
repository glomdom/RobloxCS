using RobloxCS.Types.Attributes;

namespace RobloxCS.Tests.Data;

internal class StaticTest {
    private int _x = 2;
    
    [EntryPoint]
    internal void Main() {
        var greeting = "slice";
        int a = 1, b = 2;
        int c;
        
        c = 3;
        //
        // Console.WriteLine(greeting);
        // Console.WriteLine(a);
        // Console.WriteLine(b);
        // Console.WriteLine(c);
        //
        // Echo(42);
        // Echo(c);
    }

    private static void Echo(int value) {
        // Console.WriteLine(value);
    }
}