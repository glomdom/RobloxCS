namespace RobloxCS.Renderer;

public class RendererNotFoundException : Exception {
    public string RendererName { get; }

    public RendererNotFoundException(string msg, string rendererName) : base(msg) {
        RendererName = rendererName;
    }

    public override string Message => $"{base.Message} -- culprit: {RendererName}";
}