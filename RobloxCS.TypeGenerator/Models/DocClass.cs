namespace RobloxCS.TypeGenerator.Models;

public sealed class DocClass {
    public List<DocMember>? Properties { get; set; }
    public List<DocMember>? Methods { get; set; }
}

public sealed class DocMember {
    public string Name { get; set; } = string.Empty;
    public string? Type { get; set; }
    public List<DocParameter>? Parameters { get; set; }
    public List<DocReturn>? Returns { get; set; }
}

public sealed class DocParameter {
    public string Name { get; set; } = string.Empty;
    public string? Type { get; set; }
    public object? Default { get; set; }

    public bool IsOptional => Default is not null || Type?.EndsWith('?') == true;
}

public sealed class DocReturn {
    public string? Type { get; set; }
}