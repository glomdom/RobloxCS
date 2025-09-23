namespace RobloxCS.Example;

internal class Program {
    public int A = 0;
    public int B = 0;
    public int C = 5, D = 10;
    public readonly float ReadonlyFloat = 1.5f;
    public readonly string ReadonlyString = "Hello, World!";
    public readonly int ReadonlyNumber = 2;

    public Program(string test) {
        var example = test;

        ReadonlyString = example;
    }
}