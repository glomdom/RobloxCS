using RobloxCS.Types.Attributes;

namespace RobloxCS.Server;

internal class Runtime {
    [EntryPoint]
    public void Main() {
        Console.WriteLine("Hello from server!");
    }
}