using System.Text;

namespace RobloxCS.Renderer;

public class RenderState {
    public string Indent => new(' ', _indentLevel * IndentCharacter.Length);
    public StringBuilder Builder = new();

    private const string IndentCharacter = "  ";
    private int _indentLevel;

    public void PushIndent() => _indentLevel++;
    public void PopIndent() => _indentLevel = Math.Max(0, _indentLevel - 1);
    public void AppendIndent() => Builder.Append(Indent);
    public void AppendIndented(string text) => Builder.Append($"{Indent}{text}");
    public void AppendIndentedLine(string text) => Builder.AppendLine($"{Indent}{text}");
}