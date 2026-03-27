namespace TMPPP.Domain.Structural.Bridge;

public abstract class FinanceDocument
{
    private readonly IDocumentRenderer _renderer;

    protected FinanceDocument(IDocumentRenderer renderer)
    {
        _renderer = renderer;
    }

    public string Render()
    {
        return _renderer.Render(GetTitle(), BuildLines(), GetFooter());
    }

    public string RendererName => _renderer.Name;

    protected abstract string GetTitle();
    protected abstract IReadOnlyCollection<string> BuildLines();
    protected abstract string GetFooter();
}
