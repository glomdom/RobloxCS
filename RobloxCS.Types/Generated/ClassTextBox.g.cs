#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TextBox", RobloxNativeType.Instance)]
public partial class TextBox : GuiObject  {
    public bool ClearTextOnFocus { get; } = default!;
    public string ContentText { get; } = default!;
    public int CursorPosition { get; } = default!;
    public Enums.Font FontFace { get; } = default!;
    public float LineHeight { get; } = default!;
    public int MaxVisibleGraphemes { get; } = default!;
    public bool MultiLine { get; } = default!;
    public string OpenTypeFeatures { get; } = default!;
    public string OpenTypeFeaturesError { get; } = default!;
    public Color3 PlaceholderColor3 { get; } = default!;
    public string PlaceholderText { get; } = default!;
    public bool RichText { get; } = default!;
    public int SelectionStart { get; } = default!;
    public bool ShowNativeInput { get; } = default!;
    public string Text { get; } = default!;
    public Vector2 TextBounds { get; } = default!;
    public Color3 TextColor3 { get; } = default!;
    public Enums.TextDirection TextDirection { get; } = default!;
    public bool TextEditable { get; } = default!;
    public bool TextFits { get; } = default!;
    public bool TextScaled { get; } = default!;
    public float TextSize { get; } = default!;
    public Color3 TextStrokeColor3 { get; } = default!;
    public float TextStrokeTransparency { get; } = default!;
    public float TextTransparency { get; } = default!;
    public Enums.TextTruncate TextTruncate { get; } = default!;
    public bool TextWrapped { get; } = default!;
    public Enums.TextXAlignment TextXAlignment { get; } = default!;
    public Enums.TextYAlignment TextYAlignment { get; } = default!;
    public void CaptureFocus() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public bool IsFocused() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void ReleaseFocus() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
