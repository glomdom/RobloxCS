namespace RobloxCS.Example;

internal class Program {
    public int A, B;
    public readonly float ReadonlyFloat = 0.0f;
    public readonly string ReadonlyString = "Hello, World!";
    public readonly int ReadonlyNumber = 2;

    internal Program(int a, int b) {
        A = a;
        B = b;
    }
    
    internal Program(int a, int b, int c) {
        A = a;
        B = b + c;
    }
}