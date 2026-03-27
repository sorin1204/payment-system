namespace TMPPP.Domain.Structural.Bridge;

public sealed class BridgeDemoItem
{
    public BridgeDemoItem(string documentType, string renderer, string output)
    {
        DocumentType = documentType;
        Renderer = renderer;
        Output = output;
    }

    public string DocumentType { get; }
    public string Renderer { get; }
    public string Output { get; }
}
