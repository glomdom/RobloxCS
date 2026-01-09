using RobloxCS.Types;

namespace RobloxCS.Example;

internal class Program {
    internal void Main() {
        var testPart = new Part();
        testPart.Name = "hello C#";

        var connection = testPart.Changed.Connect(val => { Console.WriteLine($"Test part changed! {val}"); });
        connection.Disconnect();

        testPart.Destroy();

        var testVector = new Vector3();
    }
}