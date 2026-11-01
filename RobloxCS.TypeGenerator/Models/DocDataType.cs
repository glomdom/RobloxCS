namespace RobloxCS.TypeGenerator.Models;

public sealed class DocDataType {
    public string Name { get; set; } = string.Empty;
    public List<DocMember>? Constructors { get; set; }
    public List<DocMember>? Constants { get; set; }
    public List<DocMember>? Properties { get; set; }
    public List<DocMember>? Methods { get; set; }
    public List<DocMember>? Functions { get; set; }
    public List<DocMathOperation>? MathOperations { get; set; }
}

public sealed class DocMathOperation {
    public string Operation { get; set; } = string.Empty;
    public string? TypeA { get; set; }
    public string? TypeB { get; set; }
    public string? ReturnType { get; set; }
}