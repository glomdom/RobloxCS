namespace RobloxCS.Example;

internal class Class1 {
    public int A = 0;

    public int TernaryTest() {
        return A > 4 ? 1 : 2;
    }

    public int DoubleA() {
        var result = DoubleNum(A);

        return result;
    }

    public int DoubleNum(int a) {
        return a * 2;
    }
}