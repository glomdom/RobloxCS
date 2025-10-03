namespace RobloxCS.Example;

internal class Class1 {
    public int A = 0;

    public int TernaryVariableTest() {
        var v = A > 4 ? 1 : 2;

        return v;
    }
    
    public int TernaryReturnTest() {
        return A > 4 ? 1 : 2;
    }
}